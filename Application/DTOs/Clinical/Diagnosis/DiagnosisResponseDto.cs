namespace Application.DTOs.Clinical.Diagnosis;

public sealed class DiagnosisResponseDto
{
    public Guid Id { get; set; }
    public Guid ClinicalAttentionId { get; set; }
    public string? Code { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
