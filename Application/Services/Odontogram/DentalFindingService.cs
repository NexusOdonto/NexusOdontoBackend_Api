using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.DentalFinding;
using Domain.Entities.Odontogram;

namespace Application.Services.Odontogram;

public sealed class DentalFindingService(
    IRepository<DentalFindingEntity> repository,
    IUnitOfWork unitOfWork) : IDentalFindingService
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

    public async Task<object> CreateAsync(CreateDentalFindingRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new DentalFindingEntity(
                request.OdontogramToothId,
                request.FindingTypeId,
                request.FindingStatusId,
                request.DentalSurfaceId,
                request.Note);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateDentalFindingRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.DentalSurfaceId, request.FindingTypeId, request.FindingStatusId, request.Note);
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

    private static object Map(DentalFindingEntity e) => new
    {
        e.Id, e.OdontogramToothId, e.DentalSurfaceId, e.FindingTypeId, e.FindingStatusId, e.Note, e.RegisteredAt
    };
}
