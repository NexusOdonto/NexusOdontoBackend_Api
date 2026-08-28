namespace Application.DTOs.Patients;

public sealed class PatientResponseDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
