namespace Application.DTOs.Appointments
{
    public class CreateAppointmentDto
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
    }
}
