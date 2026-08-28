namespace Application.DTOs.Clinical.ProcedurePerformed;

public sealed class ProcedurePerformedResponseDto
{
    public Guid Id { get; set; }
    public Guid ClinicalAttentionId { get; set; }
    public Guid? ServiceId { get; set; }
    public string? Description { get; set; }
    public DateTime PerformedAt { get; set; }
    public decimal? AmountCharged { get; set; }
}
