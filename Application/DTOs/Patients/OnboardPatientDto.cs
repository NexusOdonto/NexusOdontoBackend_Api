namespace Application.DTOs.Patients;

public sealed class OnboardPatientDto
{
    public Guid DocumentTypeId { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public Guid? SexId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }
    public string Password { get; set; } = string.Empty;
}
