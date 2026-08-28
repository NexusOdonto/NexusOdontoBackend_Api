using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Audit;
using Application.DTOs.Audit;
using Domain.Entities.Audit;

namespace Application.Services.Audit;

public sealed class AuditEventService(
    IRepository<AuditEventEntity> repository,
    IUnitOfWork unitOfWork) : IAuditEventService
{
    public async Task<IReadOnlyList<AuditEventResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<AuditEventResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<AuditEventResponseDto> CreateAsync(CreateAuditEventRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new AuditEventEntity(
                request.EventTypeId,
                request.Description,
                request.UserId,
                request.SessionId,
                request.EntityType,
                request.EntityId,
                request.PreviousData,
                request.NewData,
                request.IpAddress,
                request.UserAgent,
                request.Succeeded,
                request.ErrorDetail);
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

    private static AuditEventResponseDto Map(AuditEventEntity e) => new()
    {
        Id = e.Id,
        UserId = e.UserId,
        SessionId = e.SessionId,
        EventTypeId = e.EventTypeId,
        EntityType = e.EntityType,
        EntityId = e.EntityId,
        Description = e.Description,
        PreviousData = e.PreviousData,
        NewData = e.NewData,
        IpAddress = e.IpAddress,
        UserAgent = e.UserAgent,
        EventAt = e.EventAt,
        Succeeded = e.Succeeded,
        ErrorDetail = e.ErrorDetail
    };
}
