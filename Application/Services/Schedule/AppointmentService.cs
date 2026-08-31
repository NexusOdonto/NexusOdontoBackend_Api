using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Repositories.Schedule;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Appointments;
using Domain.Entities.Schedule;

namespace Application.Services.Schedule;

public sealed class AppointmentService(
    IAppointmentRepository repository,
    IPatientRepository patientRepository,
    IProfessionalRepository professionalRepository,
    IUnitOfWork unitOfWork) : IAppointmentService
{
    public async Task<IReadOnlyList<AppointmentResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<AppointmentResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var patientId = await ResolvePatientIdAsync(request.PatientId);
        var professionalId = await ResolveProfessionalIdAsync(request.ProfessionalId);

        var serviceId = request.ServiceId == Guid.Empty
            ? new Guid("30000000-0000-0000-0000-000000000001") // VALORACION_GENERAL
            : request.ServiceId;

        var statusId = request.AppointmentStatusId == Guid.Empty
            ? new Guid("17000000-0000-0000-0000-000000000001") // PENDIENTE
            : request.AppointmentStatusId;

        var originId = request.AppointmentOriginId == Guid.Empty
            ? new Guid("19000000-0000-0000-0000-000000000002") // CHATBOT
            : request.AppointmentOriginId;

        var startsAt = request.StartsAt == default ? DateTime.UtcNow.AddHours(1) : request.StartsAt;
        var endsAt = request.EndsAt <= startsAt ? startsAt.AddMinutes(30) : request.EndsAt;

        var overlapping = await repository.GetByProfessionalInRangeAsync(
            professionalId,
            startsAt,
            endsAt);

        if (overlapping.Any(a => a.CancelledAt == null))
        {
            throw new InvalidOperationException("El profesional ya tiene una cita agendada en ese rango de horario.");
        }

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
                string.IsNullOrWhiteSpace(request.ReasonForVisit) ? "Consulta dental programada via Chatbot" : request.ReasonForVisit,
                request.Notes);

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

        var overlapping = await repository.GetByProfessionalInRangeAsync(
            professionalId,
            startsAt,
            endsAt);

        if (overlapping.Any(a => a.Id != id && a.CancelledAt == null))
        {
            throw new InvalidOperationException("El profesional ya tiene otra cita agendada en ese rango de horario.");
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
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () => { repository.Remove(entity); return Task.CompletedTask; });
        return true;
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

    private static AppointmentResponseDto Map(AppointmentEntity e) => new()
    {
        Id = e.Id,
        PatientId = e.PatientId,
        ProfessionalId = e.ProfessionalId,
        ServiceId = e.ServiceId,
        StartsAt = e.StartsAt,
        EndsAt = e.EndsAt,
        AppointmentStatusId = e.AppointmentStatusId,
        AppointmentOriginId = e.AppointmentOriginId,
        ReasonForVisit = e.ReasonForVisit,
        Notes = e.Notes,
        CreatedByUserId = e.CreatedByUserId,
        CreatedAt = e.CreatedAt,
        CancelledAt = e.CancelledAt,
        CancellationReason = e.CancellationReason
    };
}
