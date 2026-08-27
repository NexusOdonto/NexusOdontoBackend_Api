using Application.Contracts.Repositories;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.ClinicalHistory;
using Domain.Entities.Clinical;

namespace Application.Services.Clinical;

public sealed class ClinicalHistoryService(
    IClinicalHistoryRepository repository,
    IUnitOfWork unitOfWork) : IClinicalHistoryService
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

    public async Task<object?> UpdateAsync(Guid id, UpdateClinicalHistoryRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.IsActive, request.GeneralNotes);
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
            entity.Update(false, entity.GeneralNotes);
            repository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static object Map(ClinicalHistory e) => new
    {
        e.Id, e.PatientId, e.OpenedAt, e.IsActive, e.GeneralNotes
    };
}
