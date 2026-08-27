using Application.Contracts.Repositories;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Citas;
using Domain.Entities.Schedule;

namespace Application.Services.Schedule;

public sealed class AppointmentService(
    IAppointmentRepository repository,
    IUnitOfWork unitOfWork) : IAppointmentService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreateCitaDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new Appointment(
                request.PacienteId,
                request.ProfesionalId,
                request.ServicioId,
                request.FechaHoraInicio,
                request.FechaHoraFin,
                request.EstadoCitaId,
                request.OrigenCitaId,
                request.MotivoConsulta,
                request.Observaciones);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateCitaDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.ProfessionalId = request.ProfesionalId;
            entity.ServiceId = request.ServicioId;
            entity.AppointmentOriginId = request.OrigenCitaId;
            var cancelledAt = string.IsNullOrWhiteSpace(request.MotivoCancelacion)
                ? entity.CancelledAt
                : DateTime.UtcNow;
            entity.Update(
                request.FechaHoraInicio,
                request.FechaHoraFin,
                request.EstadoCitaId,
                request.MotivoConsulta,
                request.Observaciones,
                cancelledAt,
                request.MotivoCancelacion);
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

    private static object Map(Appointment e) => new
    {
        e.Id, e.PatientId, e.ProfessionalId, e.ServiceId, e.StartsAt, e.EndsAt,
        e.AppointmentStatusId, e.AppointmentOriginId, e.ReasonForVisit, e.Notes,
        e.CreatedByUserId, e.CreatedAt, e.CancelledAt, e.CancellationReason
    };
}
