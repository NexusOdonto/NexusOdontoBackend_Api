using Application.Contracts.Repositories;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.Diagnosis;
using Domain.Entities.Clinical;

namespace Application.Services.Clinical;

public sealed class DiagnosisService(
    IRepository<Diagnosis> repository,
    IUnitOfWork unitOfWork) : IDiagnosisService
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

    public async Task<object> CreateAsync(CreateDiagnosisRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new Diagnosis(
                request.ClinicalAttentionId,
                request.Description,
                request.Code,
                string.IsNullOrWhiteSpace(request.Type) ? "DENTAL" : request.Type!);
            await repository.AddAsync(entity);
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

    private static object Map(Diagnosis e) => new
    {
        e.Id, e.ClinicalAttentionId, e.Code, e.Description, e.Type
    };
}
