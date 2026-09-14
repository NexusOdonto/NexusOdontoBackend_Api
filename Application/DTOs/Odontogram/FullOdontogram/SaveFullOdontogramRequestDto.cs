namespace Application.DTOs.Odontogram.FullOdontogram;

public sealed class SaveFullOdontogramRequestDto
{
    public Guid? PatientId { get; set; }
    public Guid? ClinicalHistoryId { get; set; }
    public Guid? ClinicalAttentionId { get; set; }
    public Guid ProfessionalId { get; set; }
    public string? Notes { get; set; }

    public List<OdontogramToothItemDto> Teeth { get; set; } = [];
    public List<DentalFindingItemDto> Findings { get; set; } = [];
}

public sealed class OdontogramToothItemDto
{
    public string ToothCode { get; set; } = null!;
    public Guid ToothStatusId { get; set; }
    public string? Note { get; set; }
}

public sealed class DentalFindingItemDto
{
    public string ToothCode { get; set; } = null!;
    public Guid? DentalSurfaceId { get; set; }
    public Guid FindingTypeId { get; set; }
    public Guid FindingStatusId { get; set; }
    public string? Note { get; set; }
}
