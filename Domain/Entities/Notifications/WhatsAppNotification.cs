using Domain.Common;
using Domain.Entities.Catalogs;
using Domain.Entities.People;
using Domain.Entities.Schedule;

namespace Domain.Entities.Notifications;

public class WhatsAppNotification : Entity
{
    public Guid? AppointmentId { get; set; }
    public Guid? PatientId { get; set; }
    public Guid WhatsAppNotificationTypeId { get; set; }
    public Guid WhatsAppNotificationStatusId { get; set; }
    public string DestinationPhone { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? TemplateName { get; set; }
    public string? TemplateParameters { get; set; }
    public string? Provider { get; set; }
    public string? ProviderMessageId { get; set; }
    public DateTime ScheduledFor { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public string? ProviderResponse { get; set; }
    public string? ErrorDetail { get; set; }

    public Appointment? Appointment { get; set; }
    public Patient? Patient { get; set; }
    public WhatsAppNotificationType? WhatsAppNotificationType { get; set; }
    public WhatsAppNotificationStatus? WhatsAppNotificationStatus { get; set; }

    private WhatsAppNotification()
    {
    }

    public WhatsAppNotification(
        Guid whatsAppNotificationTypeId,
        Guid whatsAppNotificationStatusId,
        string destinationPhone,
        string content,
        DateTime scheduledFor,
        Guid? appointmentId = null,
        Guid? patientId = null,
        string? templateName = null,
        string? templateParameters = null,
        string? provider = null)
    {
        WhatsAppNotificationTypeId = whatsAppNotificationTypeId;
        WhatsAppNotificationStatusId = whatsAppNotificationStatusId;
        DestinationPhone = destinationPhone;
        Content = content;
        ScheduledFor = scheduledFor;
        AppointmentId = appointmentId;
        PatientId = patientId;
        TemplateName = templateName;
        TemplateParameters = templateParameters;
        Provider = provider;
    }

    public void Update(
        Guid whatsAppNotificationStatusId,
        DateTime? sentAt,
        DateTime? deliveredAt,
        DateTime? readAt,
        DateTime? failedAt,
        string? providerMessageId,
        string? providerResponse,
        string? errorDetail)
    {
        WhatsAppNotificationStatusId = whatsAppNotificationStatusId;
        SentAt = sentAt;
        DeliveredAt = deliveredAt;
        ReadAt = readAt;
        FailedAt = failedAt;
        ProviderMessageId = providerMessageId;
        ProviderResponse = providerResponse;
        ErrorDetail = errorDetail;
    }
}
