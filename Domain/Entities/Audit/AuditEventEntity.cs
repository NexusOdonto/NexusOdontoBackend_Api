using Domain.Common;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Domain.Entities.Audit;

public class AuditEventEntity : Entity
{
    public Guid? UserId { get; set; }
    public Guid? SessionId { get; set; }
    public Guid EventTypeId { get; set; }
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public string Description { get; set; } = null!;
    public string? PreviousData { get; set; }
    public string? NewData { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime EventAt { get; set; } = DateTime.UtcNow;
    public bool Succeeded { get; set; } = true;
    public string? ErrorDetail { get; set; }

    public UserEntity? UserEntity { get; set; }
    public SessionEntity? SessionEntity { get; set; }
    public AuditEventTypeEntity? EventType { get; set; }

    private AuditEventEntity()
    {
    }

    public AuditEventEntity(
        Guid eventTypeId,
        string description,
        Guid? userId = null,
        Guid? sessionId = null,
        string? entityType = null,
        Guid? entityId = null,
        string? previousData = null,
        string? newData = null,
        string? ipAddress = null,
        string? userAgent = null,
        bool succeeded = true,
        string? errorDetail = null)
    {
        EventTypeId = eventTypeId;
        Description = description;
        UserId = userId;
        SessionId = sessionId;
        EntityType = entityType;
        EntityId = entityId;
        PreviousData = previousData;
        NewData = newData;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Succeeded = succeeded;
        ErrorDetail = errorDetail;
    }
}
