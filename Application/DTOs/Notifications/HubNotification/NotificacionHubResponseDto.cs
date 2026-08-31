namespace Application.DTOs.Notifications.HubNotification;

public class NotificacionHubResponseDto
{
    public Guid Id { get; set; }
    public Guid? SenderUserId { get; set; }
    public Guid? RecipientUserId { get; set; }
    public Guid? RecipientRoleId { get; set; }
    public Guid HubNotificationTypeId { get; set; }
    public Guid NotificationPriorityId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public Guid HubNotificationStatusId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }
}
