namespace Application.DTOs.Appointments;

public sealed class AppointmentClinicalContextDto
{
    public Guid AppointmentId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public Guid ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public string? ReasonForVisit { get; set; }
    public Guid AppointmentStatusId { get; set; }
    public string? AppointmentStatusCode { get; set; }
    public bool CanBeAttended { get; set; }
    public string? AttendanceStatusMessage { get; set; }

    public PatientContextDto Patient { get; set; } = null!;
    public DoctorContextDto Doctor { get; set; } = null!;
    public OdontogramBriefContextDto? LatestOdontogram { get; set; }
}

public sealed class PatientContextDto
{
    public Guid PatientId { get; set; }
    public Guid PersonId { get; set; }
    public Guid ClinicalHistoryId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string? DocumentTypeCode { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Sex { get; set; }
    public string? Address { get; set; }
}

public sealed class DoctorContextDto
{
    public Guid ProfessionalId { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid PersonId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string EmployeeCode { get; set; } = string.Empty;
    public string ProfessionalLicense { get; set; } = string.Empty;
    public List<string> Specialties { get; set; } = [];
}

public sealed class OdontogramBriefContextDto
{
    public Guid OdontogramId { get; set; }
    public DateTime RecordedAt { get; set; }
    public string? Notes { get; set; }
}
