namespace Application.DTOs.Clinical.ClinicalAttention
{
    public class UpdateClinicalAttentionRequestDto
    {
        public string? ClinicalExam { get; set; }
        public string? ProgressNotes { get; set; }
        public string? TreatmentPlan { get; set; }
    }
}
