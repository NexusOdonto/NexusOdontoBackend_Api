namespace Application.DTOs.Clinical.ClinicalHistory;

public sealed class ClinicalHistoryResponseDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public DateTime OpenedAt { get; set; }
    public bool IsActive { get; set; }
    public string? GeneralNotes { get; set; }
}
