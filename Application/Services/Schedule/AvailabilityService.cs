using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Schedule;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Disponibilidades;
using Domain.Entities.Schedule;

namespace Application.Services.Schedule;

public sealed class AvailabilityService(
    IAvailabilityRepository repository,
    IUnitOfWork unitOfWork) : IAvailabilityService
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

    public async Task<object> CreateAsync(CreateDisponibilidadDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new AvailabilityEntity(
                request.ProfesionalId,
                request.DiaInicio,
                request.DiaFin,
                request.HoraInicio,
                request.HoraAlmuerzo,
                request.HoraRetorno,
                request.HoraFin);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateDisponibilidadDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(
                request.DiaInicio,
                request.DiaFin,
                request.HoraInicio,
                request.HoraAlmuerzo,
                request.HoraRetorno,
                request.HoraFin,
                request.Activo);
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

    private static object Map(AvailabilityEntity e) => new
    {
        e.Id, e.ProfessionalId, e.StartDay, e.EndDay, e.StartTime, e.LunchStartTime, e.LunchEndTime, e.EndTime, e.IsActive
    };
}
