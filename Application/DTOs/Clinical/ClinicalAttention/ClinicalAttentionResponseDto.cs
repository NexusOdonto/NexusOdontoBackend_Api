namespace Application.DTOs.Clinical.ClinicalAttention;

public sealed class ClinicalAttentionResponseDto
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public DateTime AttentionAt { get; set; }
    public string? ClinicalExam { get; set; }
    public string? ProgressNotes { get; set; }
    public string? TreatmentPlan { get; set; }
}
