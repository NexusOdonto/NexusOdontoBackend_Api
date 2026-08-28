namespace Application.DTOs.Odontogram.OdontogramTooth;

public sealed class OdontogramToothResponseDto
{
    public Guid Id { get; set; }
    public Guid OdontogramId { get; set; }
    public string ToothCode { get; set; } = string.Empty;
    public Guid ToothStatusId { get; set; }
    public string? Note { get; set; }
}
