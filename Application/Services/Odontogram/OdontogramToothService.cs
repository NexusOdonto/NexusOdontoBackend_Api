using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.OdontogramTooth;
using Domain.Entities.Odontogram;

namespace Application.Services.Odontogram;

public sealed class OdontogramToothService(
    IRepository<OdontogramToothEntity> repository,
    IUnitOfWork unitOfWork) : IOdontogramToothService
{
    public async Task<IReadOnlyList<OdontogramToothResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<OdontogramToothResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<OdontogramToothResponseDto> CreateAsync(CreateOdontogramToothRequestDto request)
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

    public async Task<OdontogramToothResponseDto?> UpdateAsync(Guid id, UpdateOdontogramToothRequestDto request)
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

    private static OdontogramToothResponseDto Map(OdontogramToothEntity e) => new()
    {
        Id = e.Id,
        OdontogramId = e.OdontogramId,
        ToothCode = e.ToothCode,
        ToothStatusId = e.ToothStatusId,
        Note = e.Note
    };
}
