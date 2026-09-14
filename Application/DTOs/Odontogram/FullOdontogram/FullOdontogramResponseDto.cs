namespace Application.DTOs.Odontogram.FullOdontogram;

public sealed class FullOdontogramResponseDto
{
    public Guid Id { get; set; }
    public Guid ClinicalHistoryId { get; set; }
    public Guid? ClinicalAttentionId { get; set; }
    public Guid ProfessionalId { get; set; }
    public DateTime RecordedAt { get; set; }
    public string? Notes { get; set; }

    public List<FullOdontogramToothResponseDto> Teeth { get; set; } = [];
}

public sealed class FullOdontogramToothResponseDto
{
    public Guid Id { get; set; }
    public string ToothCode { get; set; } = null!;
    public Guid ToothStatusId { get; set; }
    public string? ToothStatusCode { get; set; }
    public string? ToothStatusName { get; set; }
    public string? Note { get; set; }
    public List<FullDentalFindingResponseDto> Findings { get; set; } = [];
}

public sealed class FullDentalFindingResponseDto
{
    public Guid Id { get; set; }
    public Guid? DentalSurfaceId { get; set; }
    public string? DentalSurfaceCode { get; set; }
    public Guid FindingTypeId { get; set; }
    public string? FindingTypeCode { get; set; }
    public string? FindingTypeName { get; set; }
    public string? FindingTypeColorHex { get; set; }
    public Guid FindingStatusId { get; set; }
    public string? FindingStatusCode { get; set; }
    public string? Note { get; set; }
    public DateTime RegisteredAt { get; set; }
}
