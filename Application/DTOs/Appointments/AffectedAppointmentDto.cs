namespace Application.DTOs.Appointments;

public class AffectedAppointmentDto
{
    public Guid AppointmentId { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string? PatientPhone { get; set; }
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public Guid ProfessionalId { get; set; }
    public string ProfessionalName { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public string StatusCode { get; set; } = string.Empty;
}

public sealed class RequiresRescheduleAppointmentDto : AffectedAppointmentDto
{
    public string NotificationStatus { get; set; } = string.Empty;
    public DateTime? NotifiedAt { get; set; }
}
