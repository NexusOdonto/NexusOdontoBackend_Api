using Domain.Common;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Domain.Entities.Schedule;

public class Appointment : Entity
{
    public Guid PatientId { get; set; }
    public Guid ProfessionalId { get; set; }
    public Guid ServiceId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public Guid AppointmentStatusId { get; set; }
    public Guid AppointmentOriginId { get; set; }
    public string? ReasonForVisit { get; set; }
    public string? Notes { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }

    public Patient? Patient { get; set; }
    public Professional? Professional { get; set; }
    public Service? Service { get; set; }
    public AppointmentStatus? AppointmentStatus { get; set; }
    public AppointmentOrigin? AppointmentOrigin { get; set; }
    public User? CreatedByUser { get; set; }

    private Appointment()
    {
    }

    public Appointment(
        Guid patientId,
        Guid professionalId,
        Guid serviceId,
        DateTime startsAt,
        DateTime endsAt,
        Guid appointmentStatusId,
        Guid appointmentOriginId,
        string? reasonForVisit = null,
        string? notes = null,
        Guid? createdByUserId = null)
    {
        PatientId = patientId;
        ProfessionalId = professionalId;
        ServiceId = serviceId;
        StartsAt = startsAt;
        EndsAt = endsAt;
        AppointmentStatusId = appointmentStatusId;
        AppointmentOriginId = appointmentOriginId;
        ReasonForVisit = reasonForVisit;
        Notes = notes;
        CreatedByUserId = createdByUserId;
    }

    public void Update(
        DateTime startsAt,
        DateTime endsAt,
        Guid appointmentStatusId,
        string? reasonForVisit,
        string? notes,
        DateTime? cancelledAt,
        string? cancellationReason)
    {
        UpdatedAt = DateTime.UtcNow;
        StartsAt = startsAt;
        EndsAt = endsAt;
        AppointmentStatusId = appointmentStatusId;
        ReasonForVisit = reasonForVisit;
        Notes = notes;
        CancelledAt = cancelledAt;
        CancellationReason = cancellationReason;
    }
}
