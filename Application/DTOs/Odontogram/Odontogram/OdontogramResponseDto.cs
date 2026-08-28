namespace Application.DTOs.Odontogram.Odontogram;

public sealed class OdontogramResponseDto
{
    public Guid Id { get; set; }
    public Guid ClinicalHistoryId { get; set; }
    public Guid? ClinicalAttentionId { get; set; }
    public Guid ProfessionalId { get; set; }
    public DateTime RecordedAt { get; set; }
    public string? Notes { get; set; }
}
