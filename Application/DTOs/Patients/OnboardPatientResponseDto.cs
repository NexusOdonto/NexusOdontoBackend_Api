namespace Application.DTOs.Patients;

public sealed class OnboardPatientResponseDto
{
    public Guid PersonId { get; set; }
    public Guid PatientId { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
