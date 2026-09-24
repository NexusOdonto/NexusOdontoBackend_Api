namespace Application.DTOs.Patients;

public sealed class OnboardPatientResponseDto
{
    public Guid PersonId { get; set; }
    public Guid PatientId { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    /// <summary>
    /// True when an existing person/patient was reused for this document number
    /// instead of creating a second identity.
    /// </summary>
    public bool AlreadyExisted { get; set; }
}
