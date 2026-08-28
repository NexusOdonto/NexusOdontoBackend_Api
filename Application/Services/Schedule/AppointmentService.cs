using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Schedule;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Appointments;
using Domain.Entities.Schedule;

namespace Application.Services.Schedule;

public sealed class AppointmentService(
    IAppointmentRepository repository,
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
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new AppointmentEntity(
                request.PatientId,
                request.ProfessionalId,
                request.ServiceId,
                request.StartsAt,
                request.EndsAt,
                request.AppointmentStatusId,
                request.AppointmentOriginId,
                request.ReasonForVisit,
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

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.ProfessionalId = request.ProfessionalId;
            entity.ServiceId = request.ServiceId;
            entity.AppointmentOriginId = request.AppointmentOriginId;
            var cancelledAt = string.IsNullOrWhiteSpace(request.CancellationReason)
                ? entity.CancelledAt
                : DateTime.UtcNow;
            entity.Update(
                request.StartsAt,
                request.EndsAt,
                request.AppointmentStatusId,
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
