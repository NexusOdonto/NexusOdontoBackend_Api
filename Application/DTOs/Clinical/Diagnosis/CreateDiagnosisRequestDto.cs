namespace Application.DTOs.Clinical.Diagnosis
{
    public class CreateDiagnosisRequestDto
    {
        public Guid ClinicalAttentionId { get; set; }
        public string? Code { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Type { get; set; }
    }
}
