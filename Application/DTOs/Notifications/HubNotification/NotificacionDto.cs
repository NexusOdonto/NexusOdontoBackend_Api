namespace Application.DTOs.Notifications.HubNotification;

public class NotificacionDto
{
    public Guid? EmisorUsuarioId { get; set; }
    public Guid HubNotificationTypeId { get; set; }
    public Guid NotificationPriorityId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
}
