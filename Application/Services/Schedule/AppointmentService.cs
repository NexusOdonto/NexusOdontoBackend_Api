using Application.Contracts.Repositories.Clinical;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Repositories.Schedule;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Appointments;
using Application.Exceptions;
using Domain.Entities.Catalogs;
using Domain.Entities.Clinical;
using Domain.Entities.Odontogram;
using Domain.Entities.Schedule;

namespace Application.Services.Schedule;

public sealed class AppointmentService(
    IAppointmentRepository repository,
    IPatientRepository patientRepository,
    IProfessionalRepository professionalRepository,
    IAvailabilityRepository availabilityRepository,
    IClinicalHistoryRepository clinicalHistoryRepository,
    IRepository<OdontogramEntity> odontogramRepository,
    IRepository<ServiceEntity> serviceRepository,
    IScheduleChangeService scheduleChangeService,
    IUnitOfWork unitOfWork) : IAppointmentService
{
    public static readonly Guid StatusScheduled = new("10000000-0000-0000-0000-000000000001");
    public static readonly Guid StatusConfirmed = new("10000000-0000-0000-0000-000000000002");
    public static readonly Guid StatusInWaitingRoom = new("10000000-0000-0000-0000-000000000003");
    public static readonly Guid StatusCompleted = new("10000000-0000-0000-0000-000000000004");
    public static readonly Guid StatusCancelled = new("10000000-0000-0000-0000-000000000005");
    public static readonly Guid StatusNoShow = new("10000000-0000-0000-0000-000000000006");

    public async Task<IReadOnlyList<AppointmentResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        var result = new List<AppointmentResponseDto>(items.Count);
        bool anyUpdated = false;

        foreach (var item in items)
        {
            var evaluation = EvaluateAttendanceWindow(item);
            if (evaluation.IsExpired &&
                item.AppointmentStatusId != StatusNoShow &&
                item.AppointmentStatusId != StatusCompleted &&
                item.AppointmentStatusId != StatusCancelled &&
                item.CancelledAt == null)
            {
                item.Update(
                    item.StartsAt,
                    item.EndsAt,
                    StatusNoShow,
                    item.ReasonForVisit,
                    item.Notes,
                    item.CancelledAt,
                    item.CancellationReason);
                repository.Update(item);
                anyUpdated = true;
            }
            result.Add(Map(item));
        }

        if (anyUpdated)
        {
            await unitOfWork.SaveChangesAsync();
        }

        return result;
    }

    public async Task<AppointmentResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        var evaluation = EvaluateAttendanceWindow(entity);
        if (evaluation.IsExpired &&
            entity.AppointmentStatusId != StatusNoShow &&
            entity.AppointmentStatusId != StatusCompleted &&
            entity.AppointmentStatusId != StatusCancelled &&
            entity.CancelledAt == null)
        {
            entity.Update(
                entity.StartsAt,
                entity.EndsAt,
                StatusNoShow,
                entity.ReasonForVisit,
                entity.Notes,
                entity.CancelledAt,
                entity.CancellationReason);
            repository.Update(entity);
            await unitOfWork.SaveChangesAsync();
        }

        return Map(entity);
    }

    public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var patientId = await ResolvePatientIdAsync(request.PatientId);

        var serviceId = request.ServiceId == Guid.Empty
            ? new Guid("30000000-0000-0000-0000-000000000001")
            : request.ServiceId;

        var statusId = request.AppointmentStatusId == Guid.Empty
            ? StatusScheduled
            : request.AppointmentStatusId;

        var originId = request.AppointmentOriginId == Guid.Empty
            ? new Guid("19000000-0000-0000-0000-000000000002")
            : request.AppointmentOriginId;

        // StartsAt/EndsAt are clinic wall-clock times (Colombia), not UTC.
        var nowColombia = GetColombiaNow();
        var startsAt = request.StartsAt == default ? nowColombia.AddHours(1) : request.StartsAt;
        var endsAt = request.EndsAt <= startsAt ? startsAt.AddMinutes(30) : request.EndsAt;

        ValidateAppointmentSchedule(startsAt, endsAt, nowColombia);

        var professionalId = request.AutoAssignProfessional
            ? await ResolveAvailableProfessionalAsync(request.ProfessionalId, startsAt, endsAt)
            : await ResolveAndValidateSpecificProfessionalAsync(request.ProfessionalId, startsAt, endsAt);

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new AppointmentEntity(
                patientId,
                professionalId,
                serviceId,
                startsAt,
                endsAt,
                statusId,
                originId,
                string.IsNullOrWhiteSpace(request.ReasonForVisit) ? "Consulta dental programada" : request.ReasonForVisit,
                request.Notes,
                createdByUserId: request.CreatedByUserId);

            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<AppointmentResponseDto?> UpdateAsync(Guid id, UpdateAppointmentDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        var professionalId = request.ProfessionalId == Guid.Empty ? entity.ProfessionalId : await ResolveProfessionalIdAsync(request.ProfessionalId);
        var serviceId = request.ServiceId == Guid.Empty ? entity.ServiceId : request.ServiceId;
        var originId = request.AppointmentOriginId == Guid.Empty ? entity.AppointmentOriginId : request.AppointmentOriginId;

        var startsAt = request.StartsAt == default ? entity.StartsAt : request.StartsAt;
        var endsAt = request.EndsAt <= startsAt ? startsAt.AddMinutes(30) : request.EndsAt;

        var isCancelling = request.AppointmentStatusId == StatusCancelled ||
                           !string.IsNullOrWhiteSpace(request.CancellationReason);

        var wasCancelled = entity.CancelledAt != null || entity.AppointmentStatusId == StatusCancelled;
        var attending = request.AppointmentStatusId == StatusInWaitingRoom || request.AppointmentStatusId == StatusCompleted;
        if (wasCancelled && attending)
        {
            throw new BusinessRuleException("La cita está cancelada y no puede ser atendida. Agenda una nueva cita.");
        }
        if (entity.AppointmentStatusId == StatusNoShow && attending)
        {
            throw new BusinessRuleException("El paciente no asistió a esta cita y no puede ser atendida. Agenda una nueva cita.");
        }

        var timeChanged = startsAt != entity.StartsAt || endsAt != entity.EndsAt;
        var slotChanged = timeChanged || professionalId != entity.ProfessionalId;

        if (!isCancelling)
        {
            if (timeChanged)
            {
                ValidateAppointmentSchedule(startsAt, endsAt, GetColombiaNow());
            }

            // Only re-check availability when the slot moves, so appointments left outside a
            // new schedule (pending reschedule) can still be confirmed or annotated.
            if (slotChanged && professionalId != Guid.Empty)
            {
                await ValidateAvailabilityAsync(professionalId, startsAt, endsAt);

                var overlapping = await repository.GetByProfessionalInRangeAsync(
                    professionalId,
                    startsAt,
                    endsAt);

                if (overlapping.Any(a => a.Id != id && a.CancelledAt == null && a.AppointmentStatusId != StatusCancelled && a.AppointmentStatusId != StatusNoShow))
                {
                    throw new ConflictException(
                        "The professional already has another appointment scheduled during the requested time range.");
                }
            }
        }

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.ProfessionalId = professionalId;
            entity.ServiceId = serviceId;
            entity.AppointmentOriginId = originId;
            var cancelledAt = string.IsNullOrWhiteSpace(request.CancellationReason)
                ? entity.CancelledAt
                : DateTime.UtcNow;

            entity.Update(
                startsAt,
                endsAt,
                request.AppointmentStatusId == Guid.Empty ? entity.AppointmentStatusId : request.AppointmentStatusId,
                request.ReasonForVisit,
                request.Notes,
                cancelledAt,
                request.CancellationReason);

            repository.Update(entity);

            if (isCancelling || entity.AppointmentStatusId == StatusCancelled)
            {
                await scheduleChangeService.ResolveMarksAsync([entity.Id], "cita cancelada");
            }
            else if (slotChanged)
            {
                await scheduleChangeService.ResolveMarksAsync([entity.Id], "cita reprogramada");
            }

            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            await scheduleChangeService.ResolveMarksAsync([entity.Id], "cita eliminada", detachAppointment: true);
            repository.Remove(entity);
        });
        return true;
    }

    public Task<IReadOnlyList<RequiresRescheduleAppointmentDto>> GetRequiresRescheduleAsync(CancellationToken cancellationToken = default) =>
        scheduleChangeService.GetRequiresRescheduleAsync(cancellationToken);

    public async Task<AppointmentClinicalContextDto?> GetClinicalContextAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        var appt = await repository.GetByIdAsync(appointmentId);
        if (appt is null) return null;

        var evaluation = EvaluateAttendanceWindow(appt);

        // If expired, update status in DB
        if (evaluation.IsExpired &&
            appt.AppointmentStatusId != StatusNoShow &&
            appt.AppointmentStatusId != StatusCompleted &&
            appt.AppointmentStatusId != StatusCancelled &&
            appt.CancelledAt == null)
        {
            appt.Update(
                appt.StartsAt,
                appt.EndsAt,
                StatusNoShow,
                appt.ReasonForVisit,
                appt.Notes,
                appt.CancelledAt,
                appt.CancellationReason);
            repository.Update(appt);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Get Patient with Person
        var pat = await patientRepository.GetByIdWithPersonAsync(appt.PatientId, cancellationToken);
        if (pat is null)
        {
            var allPats = await patientRepository.GetAllAsync();
            pat = allPats.FirstOrDefault(p => p.Id == appt.PatientId);
        }

        // Ensure Clinical History exists
        var clinicalHistory = await clinicalHistoryRepository.GetByPatientIdAsync(appt.PatientId, cancellationToken);
        if (clinicalHistory is null)
        {
            clinicalHistory = new ClinicalHistoryEntity(appt.PatientId, DateTime.UtcNow, "Historia clínica inicial generada automáticamente.");
            await clinicalHistoryRepository.AddAsync(clinicalHistory);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Get Doctor
        var prof = await professionalRepository.GetByIdWithSpecialtiesAsync(appt.ProfessionalId, cancellationToken)
                   ?? (await professionalRepository.GetAllAsync()).FirstOrDefault(p => p.Id == appt.ProfessionalId);

        // Get Service
        var service = await serviceRepository.GetByIdAsync(appt.ServiceId);

        // Get latest Odontogram for patient
        var allOdontograms = await odontogramRepository.GetAllAsync();
        var latestOdontogram = allOdontograms
            .Where(o => o.ClinicalHistoryId == clinicalHistory.Id)
            .OrderByDescending(o => o.RecordedAt)
            .FirstOrDefault();

        var patientPerson = pat?.PersonEntity;
        var employee = prof?.EmployeeEntity;
        var doctorPerson = employee?.PersonEntity;

        return new AppointmentClinicalContextDto
        {
            AppointmentId = appt.Id,
            StartsAt = appt.StartsAt,
            EndsAt = appt.EndsAt,
            ServiceId = appt.ServiceId,
            ServiceName = service?.Name,
            ReasonForVisit = appt.ReasonForVisit,
            AppointmentStatusId = appt.AppointmentStatusId,
            CanBeAttended = evaluation.CanAttend,
            AttendanceStatusMessage = evaluation.StatusMessage,
            Patient = new PatientContextDto
            {
                PatientId = appt.PatientId,
                PersonId = pat?.PersonId ?? Guid.Empty,
                ClinicalHistoryId = clinicalHistory.Id,
                FullName = patientPerson != null ? $"{patientPerson.FirstName} {patientPerson.LastName}".Trim() : "Paciente",
                DocumentNumber = patientPerson?.DocumentNumber ?? string.Empty,
                Email = patientPerson?.Email,
                Phone = patientPerson?.Phone,
                DateOfBirth = patientPerson?.DateOfBirth,
                Address = patientPerson?.Address
            },
            Doctor = new DoctorContextDto
            {
                ProfessionalId = appt.ProfessionalId,
                EmployeeId = prof?.EmployeeId ?? Guid.Empty,
                PersonId = employee?.PersonId ?? Guid.Empty,
                FullName = doctorPerson != null ? $"{doctorPerson.FirstName} {doctorPerson.LastName}".Trim() : "Doctor",
                EmployeeCode = employee?.EmployeeCode ?? string.Empty,
                ProfessionalLicense = prof?.ProfessionalLicense ?? string.Empty,
                Specialties = prof?.Specialties.Select(s => s.Name).ToList() ?? []
            },
            LatestOdontogram = latestOdontogram is null ? null : new OdontogramBriefContextDto
            {
                OdontogramId = latestOdontogram.Id,
                RecordedAt = latestOdontogram.RecordedAt,
                Notes = latestOdontogram.Notes
            }
        };
    }

    public async Task<AppointmentResponseDto?> MarkAsNoShowAsync(Guid appointmentId, string? reason = null, CancellationToken cancellationToken = default)
    {
        var appt = await repository.GetByIdAsync(appointmentId);
        if (appt is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var note = string.IsNullOrWhiteSpace(reason)
                ? appt.Notes
                : $"{appt.Notes} [No asistió: {reason}]".Trim();

            appt.Update(
                appt.StartsAt,
                appt.EndsAt,
                StatusNoShow,
                appt.ReasonForVisit,
                note,
                appt.CancelledAt,
                appt.CancellationReason);

            repository.Update(appt);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Map(appt);
        });
    }

    public static (bool CanAttend, string StatusMessage, bool IsExpired) EvaluateAttendanceWindow(AppointmentEntity appointment)
    {
        if (appointment.CancelledAt != null || appointment.AppointmentStatusId == StatusCancelled)
        {
            return (false, "La cita se encuentra cancelada.", false);
        }

        if (appointment.AppointmentStatusId == StatusCompleted)
        {
            return (false, "La cita ya fue atendida.", false);
        }

        if (appointment.AppointmentStatusId == StatusNoShow)
        {
            return (false, "El paciente no asistió a la cita.", true);
        }

        var nowColombia = GetColombiaNow();

        // Si la cita es de un día anterior al actual en Colombia y no fue atendida:
        if (appointment.StartsAt.Date < nowColombia.Date)
        {
            return (false, "El horario de la cita ha vencido (No asistió).", true);
        }

        // Si ya pasaron más de 45 minutos desde la hora programada sin haberse atendido
        // (StartsAt is clinic wall-clock Colombia time — compare only against Colombia "now")
        if (nowColombia > appointment.StartsAt.AddMinutes(45))
        {
            return (false, "El horario de la cita ha vencido (No asistió).", true);
        }

        // Si falta más de 15 minutos para la cita
        if (nowColombia < appointment.StartsAt.AddMinutes(-15))
        {
            return (false, "Aún no es la hora de la cita. Podrá atenderse a partir de 15 minutos antes de la hora programada.", false);
        }

        // Dentro del horario hábil de atención
        return (true, "Cita en horario de atención.", false);
    }

    /// <summary>
    /// Clinic operates in Colombia (UTC-5). Appointment StartsAt/EndsAt are stored and sent as
    /// local wall-clock values without offset, so schedule rules must use Colombia "now".
    /// </summary>
    public static DateTime GetColombiaNow()
    {
        var nowUtc = DateTime.UtcNow;
        try
        {
            var tzId = OperatingSystem.IsWindows()
                ? "SA Pacific Standard Time"
                : "America/Bogota";
            return TimeZoneInfo.ConvertTimeFromUtc(nowUtc, TimeZoneInfo.FindSystemTimeZoneById(tzId));
        }
        catch
        {
            return nowUtc.AddHours(-5);
        }
    }

    /// <summary>
    /// Past-date check (Colombia local) and clinic business hours are separate validations.
    /// Accepts an appointment only when start+duration fits before lunch or clinic close
    /// (weekdays 17:00 / Saturdays 12:00). Ending exactly at close is allowed.
    /// </summary>
    public static void ValidateAppointmentSchedule(DateTime startsAt, DateTime endsAt, DateTime? nowColombia = null)
    {
        var now = nowColombia ?? GetColombiaNow();

        // Small grace for clock skew; compare wall-clock Colombia times only.
        if (startsAt < now.AddMinutes(-5))
        {
            throw new ValidationException("No se pueden programar citas en fechas u horas pasadas.");
        }

        if (endsAt <= startsAt)
        {
            endsAt = startsAt.AddMinutes(30);
        }

        var startTime = TimeOnly.FromDateTime(startsAt);
        var endTime = TimeOnly.FromDateTime(endsAt);
        var dayOfWeek = startsAt.DayOfWeek;

        var morningOpen = new TimeOnly(8, 0);
        var lunchStart = new TimeOnly(12, 0);
        var afternoonOpen = new TimeOnly(14, 0);
        var weekdayClose = new TimeOnly(17, 0);
        var saturdayClose = new TimeOnly(12, 0);

        if (dayOfWeek == DayOfWeek.Sunday)
        {
            throw new ValidationException(
                "Los domingos la clínica permanece cerrada (día no laboral).");
        }

        if (dayOfWeek == DayOfWeek.Saturday)
        {
            if (startTime < morningOpen || startTime >= saturdayClose || endTime > saturdayClose)
            {
                throw new ValidationException(
                    "Los sábados se atiende únicamente de 08:00 a 12:00. El horario no cabe en la jornada según la duración del tratamiento.");
            }

            return;
        }

        if (startTime >= lunchStart && startTime < afternoonOpen)
        {
            throw new ValidationException(
                "El horario de 12:00 a 14:00 corresponde al receso de almuerzo clínico. La jornada es 08:00-12:00 | 14:00-17:00.");
        }

        // Morning block: must finish by lunch (12:00).
        if (startTime < lunchStart)
        {
            if (startTime < morningOpen || endTime > lunchStart)
            {
                throw new ValidationException(
                    "La cita no cabe en la jornada de la mañana (08:00-12:00) según la duración del tratamiento. La jornada es 08:00-12:00 | 14:00-17:00.");
            }

            return;
        }

        // Afternoon block: must finish by clinic close (17:00).
        if (startTime < afternoonOpen || startTime >= weekdayClose || endTime > weekdayClose)
        {
            throw new ValidationException(
                "La cita no cabe en la jornada de la tarde (14:00-17:00) según la duración del tratamiento. La jornada es 08:00-12:00 | 14:00-17:00.");
        }
    }

    private async Task<Guid> ResolveAndValidateSpecificProfessionalAsync(
        Guid professionalId,
        DateTime startsAt,
        DateTime endsAt)
    {
        var resolvedId = await ResolveProfessionalIdAsync(professionalId);
        await ValidateAvailabilityAsync(resolvedId, startsAt, endsAt);

        if (await HasActiveOverlapAsync(resolvedId, startsAt, endsAt))
        {
            throw new ConflictException(
                "The professional already has an appointment scheduled during the requested time range.");
        }

        return resolvedId;
    }

    /// <summary>
    /// Tries the preferred professional first (if provided), then any other active dentist free in the slot.
    /// </summary>
    private async Task<Guid> ResolveAvailableProfessionalAsync(
        Guid preferredProfessionalId,
        DateTime startsAt,
        DateTime endsAt)
    {
        var candidates = new List<Guid>();

        if (preferredProfessionalId != Guid.Empty)
        {
            try
            {
                candidates.Add(await ResolveProfessionalIdAsync(preferredProfessionalId));
            }
            catch (KeyNotFoundException)
            {
                // Preferred id may be stale; continue with other active professionals.
            }
        }

        var allProfessionals = await professionalRepository.GetAllAsync();
        foreach (var professional in allProfessionals
                     .Where(p => p.IsActive)
                     .OrderBy(p => p.Id))
        {
            if (!candidates.Contains(professional.Id))
            {
                candidates.Add(professional.Id);
            }
        }

        if (candidates.Count == 0)
        {
            throw new ConflictException(
                "No hay odontólogos activos disponibles para asignar la cita. Por favor selecciona otra hora.");
        }

        foreach (var candidateId in candidates)
        {
            if (await IsSlotFreeForProfessionalAsync(candidateId, startsAt, endsAt))
            {
                return candidateId;
            }
        }

        throw new ConflictException(
            "No hay odontólogos disponibles en ese horario. Por favor selecciona otra hora.");
    }

    private async Task<bool> IsSlotFreeForProfessionalAsync(
        Guid professionalId,
        DateTime startsAt,
        DateTime endsAt)
    {
        if (!await IsWithinActiveAvailabilityAsync(professionalId, startsAt, endsAt))
        {
            return false;
        }

        return !await HasActiveOverlapAsync(professionalId, startsAt, endsAt);
    }

    private async Task<bool> HasActiveOverlapAsync(Guid professionalId, DateTime startsAt, DateTime endsAt)
    {
        var overlapping = await repository.GetByProfessionalInRangeAsync(
            professionalId,
            startsAt,
            endsAt);

        return overlapping.Any(a =>
            a.CancelledAt == null &&
            a.AppointmentStatusId != StatusCancelled &&
            a.AppointmentStatusId != StatusNoShow);
    }

    private async Task<bool> IsWithinActiveAvailabilityAsync(
        Guid professionalId,
        DateTime startsAt,
        DateTime endsAt)
    {
        var availabilities = await availabilityRepository.GetByProfessionalAsync(professionalId);
        var activeAvailabilities = AvailabilityRules.EffectiveOn(availabilities, DateOnly.FromDateTime(startsAt));

        return activeAvailabilities.Count > 0 &&
               activeAvailabilities.Any(a => AvailabilityRules.IsWithinAvailability(startsAt, endsAt, a));
    }

    private async Task ValidateAvailabilityAsync(Guid professionalId, DateTime startsAt, DateTime endsAt)
    {
        if (!await IsWithinActiveAvailabilityAsync(professionalId, startsAt, endsAt))
        {
            throw new ConflictException(
                "The appointment time is outside the professional's active availability.");
        }
    }

    private async Task<Guid> ResolvePatientIdAsync(Guid inputId)
    {
        var pat = await patientRepository.GetByIdAsync(inputId);
        if (pat is not null) return pat.Id;

        pat = await patientRepository.GetByPersonIdAsync(inputId);
        if (pat is not null) return pat.Id;

        var allPats = await patientRepository.GetAllAsync();
        pat = allPats.FirstOrDefault(p => p.Id == inputId || p.PersonId == inputId);
        if (pat is not null) return pat.Id;

        throw new KeyNotFoundException($"No se encontró un paciente activo registrado con el ID '{inputId}'.");
    }

    private async Task<Guid> ResolveProfessionalIdAsync(Guid inputId)
    {
        var prof = await professionalRepository.GetByIdAsync(inputId);
        if (prof is not null) return prof.Id;

        prof = await professionalRepository.GetByEmployeeIdAsync(inputId);
        if (prof is not null) return prof.Id;

        var allProfs = await professionalRepository.GetAllAsync();
        prof = allProfs.FirstOrDefault(p =>
            p.Id == inputId ||
            p.EmployeeId == inputId ||
            (p.EmployeeEntity != null && p.EmployeeEntity.PersonId == inputId));

        if (prof is not null) return prof.Id;

        throw new KeyNotFoundException($"No se encontró un profesional odontólogo activo correspondiente al ID '{inputId}'.");
    }

    internal static string GetEffectiveStatusCode(AppointmentEntity e) =>
        GetStatusCode(GetEffectiveStatusId(e, EvaluateAttendanceWindow(e)));

    private static Guid GetEffectiveStatusId(AppointmentEntity e, (bool CanAttend, string StatusMessage, bool IsExpired) evaluation) =>
        (evaluation.IsExpired &&
            e.AppointmentStatusId != StatusNoShow &&
            e.AppointmentStatusId != StatusCompleted &&
            e.AppointmentStatusId != StatusCancelled &&
            e.CancelledAt == null) ? StatusNoShow : e.AppointmentStatusId;

    private static AppointmentResponseDto Map(AppointmentEntity e)
    {
        var evaluation = EvaluateAttendanceWindow(e);
        var effectiveStatusId = GetEffectiveStatusId(e, evaluation);

        return new AppointmentResponseDto
        {
            Id = e.Id,
            PatientId = e.PatientId,
            ProfessionalId = e.ProfessionalId,
            ServiceId = e.ServiceId,
            StartsAt = e.StartsAt,
            EndsAt = e.EndsAt,
            AppointmentStatusId = effectiveStatusId,
            AppointmentOriginId = e.AppointmentOriginId,
            ReasonForVisit = e.ReasonForVisit,
            Notes = e.Notes,
            CreatedByUserId = e.CreatedByUserId,
            CreatedAt = e.CreatedAt,
            CancelledAt = e.CancelledAt,
            CancellationReason = e.CancellationReason,
            CanBeAttended = evaluation.CanAttend,
            AttendanceStatusMessage = evaluation.StatusMessage,
            StatusName = GetStatusName(effectiveStatusId),
            StatusCode = GetStatusCode(effectiveStatusId)
        };
    }

    private static string GetStatusName(Guid statusId) => statusId switch
    {
        var id when id == StatusScheduled => "Pendiente",
        var id when id == StatusConfirmed => "Confirmada",
        var id when id == StatusInWaitingRoom => "En Atención",
        var id when id == StatusCompleted => "Completada",
        var id when id == StatusCancelled => "Cancelada",
        var id when id == StatusNoShow => "No Asistió",
        _ => "Pendiente"
    };

    private static string GetStatusCode(Guid statusId) => statusId switch
    {
        var id when id == StatusScheduled => "AGENDADA",
        var id when id == StatusConfirmed => "CONFIRMADA",
        var id when id == StatusInWaitingRoom => "EN_SALA",
        var id when id == StatusCompleted => "ATENDIDA",
        var id when id == StatusCancelled => "CANCELADA",
        var id when id == StatusNoShow => "NO_ASISTIO",
        _ => "AGENDADA"
    };
}
