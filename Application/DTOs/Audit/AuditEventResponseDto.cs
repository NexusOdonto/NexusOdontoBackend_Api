namespace Application.DTOs.Audit;

public sealed class AuditEventResponseDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid? SessionId { get; set; }
    public Guid EventTypeId { get; set; }
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? PreviousData { get; set; }
    public string? NewData { get; set; }
    public DateTime EventAt { get; set; }
    public bool Succeeded { get; set; }
    public string? ErrorDetail { get; set; }
}

public sealed class CreateAuditEventRequestDto
{
    public Guid EventTypeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public Guid? SessionId { get; set; }
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public string? PreviousData { get; set; }
    public string? NewData { get; set; }
    public bool Succeeded { get; set; } = true;
    public string? ErrorDetail { get; set; }
}
