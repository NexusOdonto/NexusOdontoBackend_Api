namespace Application.DTOs.Clinical.PatientAntecedent;

public sealed class PatientAntecedentResponseDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid AntecedentTypeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid? RegisteredByUserId { get; set; }
    public DateTime RegisteredAt { get; set; }
}
