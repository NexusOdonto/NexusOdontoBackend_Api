using System.Text.Json;
using Application.Contracts.Repositories;
using Application.Contracts.Services.Audit;
using Domain.Entities.Audit;

namespace Application.Services.Audit;

public sealed class AuditEventService(
    IRepository<AuditEvent> repository,
    IUnitOfWork unitOfWork) : IAuditEventService
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

    public async Task<object> CreateAsync(object request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var dto = Deserialize(request);

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new AuditEvent(
                dto.EventTypeId,
                dto.Description,
                dto.UserId,
                dto.SessionId,
                dto.EntityType,
                dto.EntityId,
                dto.PreviousData,
                dto.NewData,
                dto.IpAddress,
                dto.UserAgent,
                dto.Succeeded,
                dto.ErrorDetail);
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

    private static AuditPayload Deserialize(object request)
    {
        if (request is AuditPayload typed) return typed;
        var json = JsonSerializer.Serialize(request);
        return JsonSerializer.Deserialize<AuditPayload>(json)
            ?? throw new InvalidOperationException("Invalid audit event request payload.");
    }

    private static object Map(AuditEvent e) => new
    {
        e.Id, e.UserId, e.SessionId, e.EventTypeId, e.EntityType, e.EntityId,
        e.Description, e.PreviousData, e.NewData, e.IpAddress, e.UserAgent,
        e.EventAt, e.Succeeded, e.ErrorDetail
    };

    private sealed class AuditPayload
    {
        public Guid EventTypeId { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
        public Guid? SessionId { get; set; }
        public string? EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string? PreviousData { get; set; }
        public string? NewData { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool Succeeded { get; set; } = true;
        public string? ErrorDetail { get; set; }
    }
}
