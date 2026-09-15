namespace Application.DTOs.Odontogram.FullOdontogram;

public sealed class SaveFullOdontogramRequestDto
{
    public Guid? PatientId { get; set; }
    public Guid? ClinicalHistoryId { get; set; }
    public Guid? ClinicalAttentionId { get; set; }
    public Guid? ProfessionalId { get; set; }
    public string? Notes { get; set; }

    public List<OdontogramToothItemDto> Teeth { get; set; } = [];
}

public sealed class OdontogramToothItemDto
{
    /// <summary>FDI tooth number (e.g. 11, 21). Preferred by the web odontogram UI.</summary>
    public int? FdiNumber { get; set; }

    /// <summary>Alternative tooth code string (e.g. "11").</summary>
    public string? ToothCode { get; set; }

    /// <summary>Catalog tooth status code (SANO, AUSENTE, EXTRAIDO, INCLUIDO, ...).</summary>
    public string? StateCode { get; set; }

    /// <summary>Optional direct catalog id (legacy/GUID clients).</summary>
    public Guid? ToothStatusId { get; set; }

    public string? Note { get; set; }

    public List<DentalFindingItemDto> Findings { get; set; } = [];
}

public sealed class DentalFindingItemDto
{
    public string? FindingTypeCode { get; set; }
    public Guid? FindingTypeId { get; set; }
    public string? SurfaceCode { get; set; }
    public Guid? DentalSurfaceId { get; set; }
    public string? SeverityCode { get; set; }
    public Guid? FindingStatusId { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
}
