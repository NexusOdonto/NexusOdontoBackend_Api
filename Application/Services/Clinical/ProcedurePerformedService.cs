using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.ProcedurePerformed;
using Domain.Entities.Clinical;

namespace Application.Services.Clinical;

public sealed class ProcedurePerformedService(
    IRepository<ProcedurePerformedEntity> repository,
    IUnitOfWork unitOfWork) : IProcedurePerformedService
{
    public async Task<IReadOnlyList<ProcedurePerformedResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<ProcedurePerformedResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<ProcedurePerformedResponseDto> CreateAsync(CreateProcedurePerformedRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new ProcedurePerformedEntity(
                request.ClinicalAttentionId,
                request.Description,
                request.ServiceId,
                amountCharged: request.AmountCharged);
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

    private static ProcedurePerformedResponseDto Map(ProcedurePerformedEntity e) => new()
    {
        Id = e.Id,
        ClinicalAttentionId = e.ClinicalAttentionId,
        ServiceId = e.ServiceId,
        Description = e.Description,
        PerformedAt = e.PerformedAt,
        AmountCharged = e.AmountCharged
    };
}
