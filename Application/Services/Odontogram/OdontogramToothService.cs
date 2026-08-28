using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.OdontogramTooth;
using Domain.Entities.Odontogram;

namespace Application.Services.Odontogram;

public sealed class OdontogramToothService(
    IRepository<OdontogramToothEntity> repository,
    IUnitOfWork unitOfWork) : IOdontogramToothService
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

    public async Task<object> CreateAsync(CreateOdontogramToothRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new OdontogramToothEntity(
                request.OdontogramId,
                request.ToothCode,
                request.ToothStatusId,
                request.Note);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateOdontogramToothRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.ToothStatusId, request.Note);
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

    private static object Map(OdontogramToothEntity e) => new
    {
        e.Id, e.OdontogramId, e.ToothCode, e.ToothStatusId, e.Note
    };
}
