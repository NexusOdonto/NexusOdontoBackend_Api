namespace Application.DTOs.Notifications.WhatsAppNotification
{
    public class CreateWhatsAppNotificationRequestDto
    {
        public Guid? AppointmentId { get; set; }
        public Guid? PatientId { get; set; }
        public Guid WhatsAppNotificationTypeId { get; set; }
        public string DestinationPhone { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? TemplateName { get; set; }
        public string? TemplateParameters { get; set; }
        public DateTime ScheduledFor { get; set; }
    }
}
