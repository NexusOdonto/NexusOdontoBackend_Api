namespace Application.DTOs.Clinical.ClinicalAttention
{
    public class CreateClinicalAttentionRequestDto
    {
        public Guid AppointmentId { get; set; }
        public string? ClinicalExam { get; set; }
        public string? ProgressNotes { get; set; }
        public string? TreatmentPlan { get; set; }
    }
}
