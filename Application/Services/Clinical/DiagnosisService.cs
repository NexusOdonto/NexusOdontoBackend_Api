using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.Diagnosis;
using Domain.Entities.Clinical;

namespace Application.Services.Clinical;

public sealed class DiagnosisService(
    IRepository<DiagnosisEntity> repository,
    IUnitOfWork unitOfWork) : IDiagnosisService
{
    public async Task<IReadOnlyList<DiagnosisResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<DiagnosisResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<DiagnosisResponseDto> CreateAsync(CreateDiagnosisRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new DiagnosisEntity(
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

    private static DiagnosisResponseDto Map(DiagnosisEntity e) => new()
    {
        Id = e.Id,
        ClinicalAttentionId = e.ClinicalAttentionId,
        Code = e.Code,
        Description = e.Description,
        Type = e.Type
    };
}
