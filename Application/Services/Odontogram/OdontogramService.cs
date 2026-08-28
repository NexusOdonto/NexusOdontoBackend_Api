using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.Odontogram;
using Domain.Entities.Odontogram;

namespace Application.Services.Odontogram;

public sealed class OdontogramService(
    IRepository<Domain.Entities.Odontogram.OdontogramEntity> repository,
    IUnitOfWork unitOfWork) : IOdontogramService
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

    public async Task<object> CreateAsync(CreateOdontogramRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new Domain.Entities.Odontogram.OdontogramEntity(
                request.ClinicalHistoryId,
                request.ProfessionalId,
                request.ClinicalAttentionId,
                request.Notes);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateOdontogramRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.Notes, entity.ClinicalAttentionId);
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

    private static object Map(Domain.Entities.Odontogram.OdontogramEntity e) => new
    {
        e.Id, e.ClinicalHistoryId, e.ClinicalAttentionId, e.ProfessionalId, e.RecordedAt, e.Notes
    };
}
