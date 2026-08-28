namespace Application.DTOs.Notifications.WhatsAppNotification;

public sealed class WhatsAppNotificationResponseDto
{
    public Guid Id { get; set; }
    public Guid? AppointmentId { get; set; }
    public Guid? PatientId { get; set; }
    public Guid WhatsAppNotificationTypeId { get; set; }
    public Guid WhatsAppNotificationStatusId { get; set; }
    public string DestinationPhone { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? TemplateName { get; set; }
    public string? TemplateParameters { get; set; }
    public DateTime? ScheduledFor { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? FailedAt { get; set; }
}
