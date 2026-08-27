using Application.Contracts.Repositories;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.ClinicalAttention;
using Domain.Entities.Clinical;

namespace Application.Services.Clinical;

public sealed class ClinicalAttentionService(
    IClinicalAttentionRepository repository,
    IUnitOfWork unitOfWork) : IClinicalAttentionService
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

    public async Task<object> CreateAsync(CreateClinicalAttentionRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new ClinicalAttention(
                request.AppointmentId,
                clinicalExam: request.ClinicalExam,
                progressNotes: request.ProgressNotes,
                treatmentPlan: request.TreatmentPlan);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateClinicalAttentionRequestDto request)
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

    private static object Map(ClinicalAttention e) => new
    {
        e.Id, e.AppointmentId, e.AttentionAt, e.ClinicalExam, e.ProgressNotes, e.TreatmentPlan
    };
}
