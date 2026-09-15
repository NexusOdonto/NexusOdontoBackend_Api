namespace Application.DTOs.Appointments
{
    public class CreateAppointmentDto
    {
        public Guid PatientId { get; set; }
        /// <summary>
        /// Preferred professional. When <see cref="AutoAssignProfessional"/> is true this is optional
        /// and used only as the first candidate before trying other active dentists.
        /// </summary>
        public Guid ProfessionalId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public Guid AppointmentStatusId { get; set; }
        public Guid AppointmentOriginId { get; set; }
        public string? ReasonForVisit { get; set; }
        public string? Notes { get; set; }
        public Guid? CreatedByUserId { get; set; }
        /// <summary>
        /// When true, if the preferred professional is busy/unavailable the API assigns the first
        /// other active professional free in the slot. Used by patient self-booking (no doctor picker).
        /// </summary>
        public bool AutoAssignProfessional { get; set; }
    }
}
