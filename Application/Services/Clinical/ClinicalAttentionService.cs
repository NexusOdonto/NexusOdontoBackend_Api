using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Clinical;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.ClinicalAttention;
using Domain.Entities.Clinical;

namespace Application.Services.Clinical;

public sealed class ClinicalAttentionService(
    IClinicalAttentionRepository repository,
    IUnitOfWork unitOfWork) : IClinicalAttentionService
{
    public async Task<IReadOnlyList<ClinicalAttentionResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<ClinicalAttentionResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<ClinicalAttentionResponseDto> CreateAsync(CreateClinicalAttentionRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new ClinicalAttentionEntity(
                request.AppointmentId,
                clinicalExam: request.ClinicalExam,
                progressNotes: request.ProgressNotes,
                treatmentPlan: request.TreatmentPlan);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<ClinicalAttentionResponseDto?> UpdateAsync(Guid id, UpdateClinicalAttentionRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.ClinicalExam, request.ProgressNotes, request.TreatmentPlan);
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

    private static ClinicalAttentionResponseDto Map(ClinicalAttentionEntity e) => new()
    {
        Id = e.Id,
        AppointmentId = e.AppointmentId,
        AttentionAt = e.AttentionAt,
        ClinicalExam = e.ClinicalExam,
        ProgressNotes = e.ProgressNotes,
        TreatmentPlan = e.TreatmentPlan
    };
}
