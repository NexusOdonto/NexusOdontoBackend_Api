using System.Globalization;
using System.Text.Json;
using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Notifications;
using Application.Contracts.Repositories.People;
using Application.Contracts.Repositories.Schedule;
using Application.Contracts.Repositories.Security;
using Application.Contracts.Services.Chatbot;
using Application.Contracts.Services.Notifications;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Appointments;
using Application.DTOs.Availabilities;
using Application.DTOs.Chats.ScheduleChange;
using Application.DTOs.Notifications.HubNotification;
using Application.Exceptions;
using Domain.Entities.Catalogs;
using Domain.Entities.Notifications;
using Domain.Entities.People;
using Domain.Entities.Schedule;
using Microsoft.Extensions.Logging;

namespace Application.Services.Schedule;

public sealed class AvailabilityService(
    IAvailabilityRepository repository,
    IProfessionalRepository professionalRepository,
    IAppointmentRepository appointmentRepository,
    IWhatsAppNotificationRepository notificationRepository,
    IScheduleChangeService scheduleChangeService,
    IChatbotClient chatbotClient,
    INotificationPublisherService notificationPublisher,
    IRoleRepository roleRepository,
    IEmployeeRepository employeeRepository,
    IUserRepository userRepository,
    IRepository<PersonEntity> personRepository,
    ICatalogRepository<HubNotificationTypeEntity> hubTypeCatalog,
    ICatalogRepository<NotificationPriorityEntity> priorityCatalog,
    IUnitOfWork unitOfWork,
    ILogger<AvailabilityService> logger) : IAvailabilityService
{
    private const int ImpactWindowDays = 90;
    private const int MaxParallelBotCalls = 4;
    private const string NoPhonePlaceholder = "SIN_TELEFONO";

    public async Task<IReadOnlyList<AffectedAppointmentDto>> GetImpactAsync(
        AvailabilityImpactRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var rows = ValidateRows(request.Rows);
        var professionalId = await ResolveProfessionalIdAsync(request.ProfessionalId);

        var (affected, _) = await FindAffectedAppointmentsAsync(professionalId, rows, GetEffectiveFromDate(), cancellationToken);
        return await scheduleChangeService.BuildAffectedAsync(affected.Select(a => a.Id).ToList(), cancellationToken);
    }

    public async Task<IReadOnlyList<AvailabilityResponseDto>> GetEffectiveForProfessionalAsync(
        Guid professionalId,
        DateOnly? date,
        CancellationToken cancellationToken = default)
    {
        var resolvedProfessionalId = await ResolveProfessionalIdAsync(professionalId);
        var day = date ?? GetColombiaToday();
        var rows = await repository.GetByProfessionalAsync(resolvedProfessionalId, cancellationToken);
        return AvailabilityRules.EffectiveOn(rows, day)
            .OrderBy(r => r.StartDay)
            .Select(Map)
            .ToList();
    }

    private static DateOnly GetColombiaToday() => DateOnly.FromDateTime(AppointmentService.GetColombiaNow());

    /// <summary>Schedule changes never apply the same day: they start the next Colombia day.</summary>
    private static DateOnly GetEffectiveFromDate() => GetColombiaToday().AddDays(1);

    public async Task<ReplaceProfessionalAvailabilityResultDto> ReplaceForProfessionalAsync(
        Guid professionalId,
        ReplaceProfessionalAvailabilityDto request,
        Guid? requestedByUserId = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var rows = ValidateRows(request.Rows);
        var resolvedProfessionalId = await ResolveProfessionalIdAsync(professionalId);
        var today = GetColombiaToday();
        var effectiveFrom = today.AddDays(1);

        var (affectedEntities, upcoming) = await FindAffectedAppointmentsAsync(resolvedProfessionalId, rows, effectiveFrom, cancellationToken);
        var affectedIds = affectedEntities.Select(a => a.Id).ToList();
        var affected = await scheduleChangeService.BuildAffectedAsync(affectedIds, cancellationToken);
        var catalogIds = await scheduleChangeService.GetCatalogIdsAsync(cancellationToken);

        var botPhones = new Dictionary<Guid, string?>();
        foreach (var item in affected)
        {
            botPhones[item.AppointmentId] = await scheduleChangeService.ResolveBotPhoneAsync(
                item.PatientId, item.PatientPhone, cancellationToken);
        }

        var nowUtc = DateTime.UtcNow;
        var marks = new Dictionary<Guid, WhatsAppNotificationEntity>();
        var savedRows = new List<AvailabilityEntity>();
        var reverted = false;
        var hadPending = false;

        await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var current = await repository.GetByProfessionalAsync(resolvedProfessionalId, cancellationToken);
            var currentToday = AvailabilityRules.EffectiveOn(current, today);

            // A second save the same day overrides the change still waiting to start.
            var pending = current.Where(a => a.IsActive && a.EffectiveFrom > today).ToList();
            hadPending = pending.Count > 0;
            foreach (var p in pending)
            {
                p.IsActive = false;
                repository.Update(p);
            }

            if (currentToday.Count > 0 && SameWeeklySchedule(currentToday, rows))
            {
                // Saving the schedule already in force just keeps it open-ended.
                reverted = true;
                foreach (var r in currentToday.Where(r => r.EffectiveTo is not null))
                {
                    r.EffectiveTo = null;
                    repository.Update(r);
                }
                savedRows.AddRange(currentToday);
            }
            else
            {
                // Today's rows keep applying until the end of today (inclusive).
                foreach (var r in currentToday.Where(r => r.EffectiveTo is null || r.EffectiveTo > today))
                {
                    r.EffectiveTo = today;
                    repository.Update(r);
                }

                foreach (var row in rows)
                {
                    var entity = new AvailabilityEntity(
                        resolvedProfessionalId,
                        row.StartDay,
                        row.EndDay,
                        row.StartTime,
                        row.LunchStartTime,
                        row.LunchEndTime,
                        row.EndTime)
                    {
                        EffectiveFrom = effectiveFrom
                    };
                    await repository.AddAsync(entity);
                    savedRows.Add(entity);
                }
            }

            // Previously marked appointments that fit the new schedule no longer need rescheduling;
            // still-affected ones get a fresh mark below.
            var upcomingIds = upcoming.Select(a => a.Id).ToList();
            await scheduleChangeService.ResolveMarksAsync(upcomingIds, "horario actualizado", cancellationToken: cancellationToken);

            foreach (var item in affected)
            {
                var phone = botPhones[item.AppointmentId];
                var mark = new WhatsAppNotificationEntity(
                    catalogIds.ScheduleChangeTypeId,
                    catalogIds.PendingStatusId,
                    Truncate(string.IsNullOrWhiteSpace(phone) ? NoPhonePlaceholder : phone, 30),
                    BuildMarkContent(item),
                    nowUtc,
                    appointmentId: item.AppointmentId,
                    patientId: item.PatientId,
                    templateName: "schedule_change",
                    templateParameters: JsonSerializer.Serialize(new
                    {
                        professionalId = item.ProfessionalId,
                        startsAt = item.StartsAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                        requestedByUserId
                    }),
                    provider: "chatbot");
                await notificationRepository.AddAsync(mark);
                marks[item.AppointmentId] = mark;
            }
        });

        var notified = 0;
        var failed = 0;

        if (request.NotifyPatients && affected.Count > 0)
        {
            (notified, failed) = await NotifyPatientsAsync(affected, botPhones, marks, catalogIds, cancellationToken);
        }

        if (affected.Count > 0)
        {
            await PublishReceptionSummaryAsync(affected, notified, failed, request.NotifyPatients, requestedByUserId);
        }

        var notifiedVia = await NotifyProfessionalAsync(
            resolvedProfessionalId,
            rows,
            effectiveFrom,
            reverted,
            hadPending,
            affected.Count,
            notified,
            failed,
            request.NotifyPatients,
            requestedByUserId,
            cancellationToken);

        return new ReplaceProfessionalAvailabilityResultDto
        {
            Rows = savedRows.Select(Map).ToList(),
            Affected = affected,
            Notified = notified,
            Failed = failed,
            EffectiveFrom = effectiveFrom,
            RevertedToCurrent = reverted,
            ProfessionalNotifiedVia = notifiedVia
        };
    }

    /// <summary>
    /// Tells the professional (their own user account) that their schedule changed. Falls back to the
    /// admin and reception roles when the professional has no linked user.
    /// </summary>
    private async Task<string> NotifyProfessionalAsync(
        Guid professionalId,
        IReadOnlyList<AvailabilityRowDto> rows,
        DateOnly effectiveFrom,
        bool reverted,
        bool hadPending,
        int affectedCount,
        int notified,
        int failed,
        bool notifyPatients,
        Guid? requestedByUserId,
        CancellationToken cancellationToken)
    {
        try
        {
            var hubType = await hubTypeCatalog.GetByCodeAsync("HORARIO_ACTUALIZADO")
                ?? await hubTypeCatalog.GetByCodeAsync("CITA_CANCELADA")
                ?? (await hubTypeCatalog.GetAllAsync()).FirstOrDefault();
            var priority = await priorityCatalog.GetByCodeAsync(affectedCount > 0 ? "ALTA" : "NORMAL")
                ?? (await priorityCatalog.GetAllAsync()).FirstOrDefault();
            if (hubType is null || priority is null) return "none";

            var (userId, professionalName) = await ResolveProfessionalUserAsync(professionalId, cancellationToken);

            var schedule = ScheduleText.Describe(rows);
            var dateText = ScheduleText.FormatTomorrow(effectiveFrom);
            var patientsText = BuildPatientsText(affectedCount, notified, failed, notifyPatients);

            string title;
            string body;
            if (reverted && hadPending)
            {
                title = "Se canceló tu cambio de horario";
                body = $"El cambio de horario que iba a regir desde {dateText} se canceló. Sigues con tu horario actual: {schedule}.";
            }
            else if (reverted)
            {
                title = "Tu horario se guardó sin cambios";
                body = $"Tu horario se guardó sin cambios: {schedule}.";
            }
            else
            {
                title = "Tu horario cambió";
                body = $"Tu horario cambió. Rige desde {dateText}: {schedule}. Hoy sigues con tu horario anterior.";
            }

            if (userId is Guid recipient)
            {
                await notificationPublisher.SendNotificationToUserAsync(recipient, new NotificacionDto
                {
                    EmisorUsuarioId = requestedByUserId,
                    HubNotificationTypeId = hubType.Id,
                    NotificationPriorityId = priority.Id,
                    Title = title,
                    Content = Truncate($"{body} {patientsText}", 1000),
                    EntityType = "PROFESSIONAL_SCHEDULE",
                    EntityId = professionalId
                });
                return "user";
            }

            logger.LogWarning(
                "El profesional {ProfessionalId} no tiene usuario vinculado; el aviso de cambio de horario va a administración y recepción.",
                professionalId);

            var fallback = new NotificacionDto
            {
                EmisorUsuarioId = requestedByUserId,
                HubNotificationTypeId = hubType.Id,
                NotificationPriorityId = priority.Id,
                Title = $"{title} ({professionalName})",
                Content = Truncate($"Horario de {professionalName}. {body} {patientsText}", 1000),
                EntityType = "PROFESSIONAL_SCHEDULE",
                EntityId = professionalId
            };
            var sent = false;
            foreach (var code in new[] { "ADMINISTRADOR", "RECEPCIONISTA" })
            {
                var role = await roleRepository.GetByCodeAsync(code);
                if (role is null) continue;
                await notificationPublisher.SendNotificationToRoleAsync(role.Id, fallback);
                sent = true;
            }
            return sent ? "roles" : "none";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No se pudo notificar al profesional {ProfessionalId} el cambio de horario.", professionalId);
            return "none";
        }
    }

    private async Task<(Guid? UserId, string Name)> ResolveProfessionalUserAsync(Guid professionalId, CancellationToken cancellationToken)
    {
        var professional = await professionalRepository.GetByIdAsync(professionalId);
        if (professional is null) return (null, "el odontólogo");

        var employee = await employeeRepository.GetByIdAsync(professional.EmployeeId);
        if (employee is null) return (null, "el odontólogo");

        var person = await personRepository.GetByIdAsync(employee.PersonId);
        var name = person is null ? "el odontólogo" : $"{person.FirstName} {person.LastName}".Trim();

        var user = await userRepository.GetByPersonIdAsync(employee.PersonId, cancellationToken);
        return (user is { IsActive: true } ? user.Id : null, name);
    }

    private static string BuildPatientsText(int affectedCount, int notified, int failed, bool notifyPatients)
    {
        if (affectedCount == 0)
            return "Ninguna cita queda por fuera del nuevo horario. Las citas de hoy no cambian.";

        var citas = affectedCount == 1 ? "1 cita" : $"{affectedCount} citas";
        var quedan = affectedCount == 1 ? "queda" : "quedan";
        if (!notifyPatients)
            return $"{citas} desde mañana {quedan} por fuera y {(affectedCount == 1 ? "quedó marcada" : "quedaron marcadas")} como \"Requiere reprogramación\" sin aviso al paciente. Las citas de hoy no cambian.";

        var avisos = failed > 0
            ? $"avisamos por WhatsApp a {notified} paciente(s) y {failed} aviso(s) no se pudieron enviar"
            : $"avisamos por WhatsApp a {notified} paciente(s) para reprogramar";
        return $"{citas} desde mañana {quedan} por fuera; {avisos}. Las citas de hoy no cambian.";
    }

    /// <summary>Compares two weekly schedules day by day, regardless of how the rows group the days.</summary>
    private static bool SameWeeklySchedule(IReadOnlyList<AvailabilityEntity> current, IReadOnlyList<AvailabilityRowDto> proposed)
    {
        static string Slot(TimeOnly start, TimeOnly lunchStart, TimeOnly lunchEnd, TimeOnly end) =>
            lunchStart < lunchEnd
                ? $"{start:HH\\:mm}-{lunchStart:HH\\:mm}-{lunchEnd:HH\\:mm}-{end:HH\\:mm}"
                : $"{start:HH\\:mm}-{end:HH\\:mm}";

        for (var day = 1; day <= 7; day++)
        {
            var a = current
                .Where(r => r.StartDay <= day && day <= r.EndDay)
                .Select(r => Slot(r.StartTime, r.LunchStartTime, r.LunchEndTime, r.EndTime))
                .Distinct()
                .OrderBy(s => s, StringComparer.Ordinal);
            var b = proposed
                .Where(r => r.StartDay <= day && day <= r.EndDay)
                .Select(r => Slot(r.StartTime, r.LunchStartTime, r.LunchEndTime, r.EndTime))
                .Distinct()
                .OrderBy(s => s, StringComparer.Ordinal);
            if (!a.SequenceEqual(b)) return false;
        }

        return true;
    }

    private async Task<(List<AppointmentEntity> Affected, List<AppointmentEntity> Upcoming)> FindAffectedAppointmentsAsync(
        Guid professionalId,
        IReadOnlyList<AvailabilityRowDto> rows,
        DateOnly effectiveFrom,
        CancellationToken cancellationToken)
    {
        // Appointments before the new schedule starts (today's) keep the schedule they were booked under.
        var from = effectiveFrom.ToDateTime(TimeOnly.MinValue);
        var inRange = await appointmentRepository.GetByProfessionalInRangeAsync(
            professionalId, from, from.AddDays(ImpactWindowDays), cancellationToken);

        var upcoming = inRange
            .Where(a =>
                a.StartsAt >= from &&
                a.CancelledAt == null &&
                (a.AppointmentStatusId == AppointmentService.StatusScheduled ||
                 a.AppointmentStatusId == AppointmentService.StatusConfirmed))
            .OrderBy(a => a.StartsAt)
            .ToList();

        var affected = upcoming
            .Where(a => !rows.Any(r => AvailabilityRules.IsWithinAvailability(
                a.StartsAt, a.EndsAt, r.StartDay, r.EndDay, r.StartTime, r.LunchStartTime, r.LunchEndTime, r.EndTime)))
            .ToList();

        return (affected, upcoming);
    }

    private async Task<(int Notified, int Failed)> NotifyPatientsAsync(
        IReadOnlyList<AffectedAppointmentDto> affected,
        IReadOnlyDictionary<Guid, string?> botPhones,
        IReadOnlyDictionary<Guid, WhatsAppNotificationEntity> marks,
        ScheduleChangeCatalogIds catalogIds,
        CancellationToken cancellationToken)
    {
        using var gate = new SemaphoreSlim(MaxParallelBotCalls);

        var calls = affected.Select(async item =>
        {
            var phone = botPhones[item.AppointmentId];
            if (string.IsNullOrWhiteSpace(phone))
            {
                return (item.AppointmentId, Result: new ScheduleChangeNotifyResult
                {
                    Success = false,
                    Error = "El paciente no tiene teléfono ni conversación de WhatsApp registrada."
                });
            }

            await gate.WaitAsync(cancellationToken);
            try
            {
                var result = await chatbotClient.NotifyScheduleChangeAsync(new ScheduleChangeNotificationDto
                {
                    AppointmentId = item.AppointmentId,
                    PatientId = item.PatientId,
                    PatientName = item.PatientName,
                    Phone = phone,
                    ServiceId = item.ServiceId,
                    Service = item.ServiceName,
                    ProfessionalId = item.ProfessionalId,
                    Professional = item.ProfessionalName,
                    StartsAt = item.StartsAt,
                    EndsAt = item.EndsAt
                }, cancellationToken);
                return (item.AppointmentId, Result: result);
            }
            finally
            {
                gate.Release();
            }
        });

        var results = await Task.WhenAll(calls);

        var notified = 0;
        var failed = 0;
        var nowUtc = DateTime.UtcNow;

        foreach (var (appointmentId, result) in results)
        {
            var mark = marks[appointmentId];
            if (result.Success)
            {
                notified++;
                var queued = result.Status == "queued";
                mark.Update(
                    queued ? catalogIds.PendingStatusId : catalogIds.SentStatusId,
                    queued ? null : nowUtc,
                    mark.DeliveredAt,
                    mark.ReadAt,
                    null,
                    mark.ProviderMessageId,
                    result.Status,
                    null);
            }
            else
            {
                failed++;
                mark.Update(
                    catalogIds.FailedStatusId,
                    mark.SentAt,
                    mark.DeliveredAt,
                    mark.ReadAt,
                    nowUtc,
                    mark.ProviderMessageId,
                    result.Status,
                    Truncate(result.Error ?? "Error desconocido", 1000));
            }

            notificationRepository.Update(mark);
        }

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No se pudo guardar el estado de los avisos de cambio de horario.");
        }

        return (notified, failed);
    }

    private async Task PublishReceptionSummaryAsync(
        IReadOnlyList<AffectedAppointmentDto> affected,
        int notified,
        int failed,
        bool notifyPatients,
        Guid? requestedByUserId)
    {
        try
        {
            var reception = await roleRepository.GetByCodeAsync("RECEPCIONISTA");
            if (reception is null) return;

            var hubType = await hubTypeCatalog.GetByCodeAsync("CITA_CANCELADA")
                ?? (await hubTypeCatalog.GetAllAsync()).FirstOrDefault();
            var priority = await priorityCatalog.GetByCodeAsync("ALTA")
                ?? (await priorityCatalog.GetAllAsync()).FirstOrDefault();
            if (hubType is null || priority is null) return;

            var professionalName = affected[0].ProfessionalName;
            var count = affected.Count;
            var content = notifyPatients
                ? $"El horario de {professionalName} cambió y {count} cita(s) quedaron por fuera. Avisadas por WhatsApp: {notified}. Con error: {failed}. Revísalas en \"Requiere reprogramación\"."
                : $"El horario de {professionalName} cambió y {count} cita(s) quedaron por fuera sin aviso al paciente. Revísalas en \"Requiere reprogramación\".";

            await notificationPublisher.SendNotificationToRoleAsync(reception.Id, new NotificacionDto
            {
                EmisorUsuarioId = requestedByUserId,
                HubNotificationTypeId = hubType.Id,
                NotificationPriorityId = priority.Id,
                Title = $"Cambio de horario: {count} cita(s) requieren reprogramación",
                Content = content,
                EntityType = "PROFESSIONAL_SCHEDULE",
                EntityId = affected[0].ProfessionalId
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No se pudo notificar a recepción el cambio de horario.");
        }
    }

    private static List<AvailabilityRowDto> ValidateRows(IReadOnlyList<AvailabilityRowDto>? rows)
    {
        var list = rows?.ToList() ?? [];
        var errors = new Dictionary<string, string[]>();

        for (var i = 0; i < list.Count; i++)
        {
            var r = list[i];
            var messages = new List<string>();

            if (r.StartDay is < 1 or > 7 || r.EndDay is < 1 or > 7)
                messages.Add("startDay y endDay deben estar entre 1 (lunes) y 7 (domingo).");
            else if (r.StartDay > r.EndDay)
                messages.Add("startDay no puede ser mayor que endDay.");

            if (r.StartTime >= r.EndTime)
                messages.Add("startTime debe ser anterior a endTime.");

            if (r.LunchStartTime > r.LunchEndTime)
                messages.Add("lunchStartTime no puede ser posterior a lunchEndTime.");
            else if (r.LunchStartTime < r.LunchEndTime &&
                     (r.LunchStartTime < r.StartTime || r.LunchEndTime > r.EndTime))
                messages.Add("El almuerzo debe estar dentro de la jornada.");

            if (messages.Count > 0)
                errors[$"rows[{i}]"] = messages.ToArray();
        }

        if (errors.Count > 0)
            throw new ValidationException(errors);

        return list;
    }

    private static string BuildMarkContent(AffectedAppointmentDto item) =>
        $"Cambio de horario: la cita de {item.ServiceName} de {item.PatientName} con {item.ProfessionalName} " +
        $"el {item.StartsAt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)} quedó fuera del nuevo horario.";

    private static string Truncate(string value, int max) =>
        value.Length <= max ? value : value[..max];

    public async Task<IReadOnlyList<AvailabilityResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<AvailabilityResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<AvailabilityResponseDto> CreateAsync(CreateAvailabilityDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var professionalId = await ResolveProfessionalIdAsync(request.ProfessionalId);

        var startTime = ResolveTime(request.StartTime, new TimeOnly(8, 0));
        var lunchStartTime = ResolveTime(request.LunchStartTime, new TimeOnly(12, 0));
        var lunchEndTime = ResolveTime(request.LunchEndTime, new TimeOnly(12, 0));
        var endTime = ResolveTime(request.EndTime, new TimeOnly(17, 0));

        var startDay = request.StartDay is >= 1 and <= 7 ? request.StartDay : 1;
        var endDay = request.EndDay is >= 1 and <= 7 ? request.EndDay : 5;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new AvailabilityEntity(
                professionalId,
                startDay,
                endDay,
                startTime,
                lunchStartTime,
                lunchEndTime,
                endTime);

            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<AvailabilityResponseDto?> UpdateAsync(Guid id, UpdateAvailabilityDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        var startTime = request.StartTime;
        var lunchStartTime = request.LunchStartTime;
        var lunchEndTime = request.LunchEndTime;
        var endTime = request.EndTime;

        var startDay = request.StartDay is >= 1 and <= 7 ? request.StartDay : entity.StartDay;
        var endDay = request.EndDay is >= 1 and <= 7 ? request.EndDay : entity.EndDay;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(
                startDay,
                endDay,
                startTime,
                lunchStartTime,
                lunchEndTime,
                endTime,
                request.IsActive);

            repository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Update(entity.StartDay, entity.EndDay, entity.StartTime, entity.LunchStartTime, entity.LunchEndTime, entity.EndTime, false);
            repository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private async Task<Guid> ResolveProfessionalIdAsync(Guid inputId)
    {
        // 1. Intentar directamente como ProfessionalId
        var prof = await professionalRepository.GetByIdAsync(inputId);
        if (prof is not null) return prof.Id;

        // 2. Intentar como EmployeeId
        prof = await professionalRepository.GetByEmployeeIdAsync(inputId);
        if (prof is not null) return prof.Id;

        // 3. Buscar en la lista general por PersonId o EmployeeId
        var all = await professionalRepository.GetAllAsync();
        prof = all.FirstOrDefault(p =>
            p.Id == inputId ||
            p.EmployeeId == inputId ||
            (p.EmployeeEntity != null && p.EmployeeEntity.PersonId == inputId));

        if (prof is not null) return prof.Id;

        throw new KeyNotFoundException($"No se encontró un profesional odontólogo activo correspondiente al ID '{inputId}'. Por favor verifique el ID del profesional.");
    }

    private static TimeOnly ResolveTime(TimeOnly time, TimeOnly fallback) =>
        time == default ? fallback : time;

    private static AvailabilityResponseDto Map(AvailabilityEntity e) => new()
    {
        Id = e.Id,
        ProfessionalId = e.ProfessionalId,
        StartDay = e.StartDay,
        EndDay = e.EndDay,
        StartTime = e.StartTime,
        LunchStartTime = e.LunchStartTime,
        LunchEndTime = e.LunchEndTime,
        EndTime = e.EndTime,
        IsActive = e.IsActive,
        EffectiveFrom = e.EffectiveFrom,
        EffectiveTo = e.EffectiveTo
    };
}
