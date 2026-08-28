namespace Application.DTOs.Odontogram.DentalFinding;

public sealed class DentalFindingResponseDto
{
    public Guid Id { get; set; }
    public Guid OdontogramToothId { get; set; }
    public Guid? DentalSurfaceId { get; set; }
    public Guid FindingTypeId { get; set; }
    public Guid FindingStatusId { get; set; }
    public string? Note { get; set; }
    public DateTime RegisteredAt { get; set; }
}
