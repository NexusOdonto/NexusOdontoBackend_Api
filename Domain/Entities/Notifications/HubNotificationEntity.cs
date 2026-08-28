using Domain.Common;
using Domain.Entities.Catalogs;
using Domain.Entities.People;
using Domain.Entities.Security;

namespace Domain.Entities.Notifications;

public class HubNotificationEntity : Entity
{
    public Guid? SenderUserId { get; set; }
    public Guid? RecipientUserId { get; set; }
    public Guid? RecipientRoleId { get; set; }
    public Guid HubNotificationTypeId { get; set; }
    public Guid NotificationPriorityId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public Guid HubNotificationStatusId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }

    public UserEntity? SenderUser { get; set; }
    public UserEntity? RecipientUser { get; set; }
    public RoleEntity? RecipientRole { get; set; }
    public HubNotificationTypeEntity? HubNotificationTypeEntity { get; set; }
    public NotificationPriorityEntity? NotificationPriorityEntity { get; set; }
    public HubNotificationStatusEntity? HubNotificationStatusEntity { get; set; }

    private HubNotificationEntity()
    {
    }

    public HubNotificationEntity(
        Guid hubNotificationTypeId,
        Guid notificationPriorityId,
        string title,
        string content,
        Guid hubNotificationStatusId,
        Guid? senderUserId = null,
        Guid? recipientUserId = null,
        Guid? recipientRoleId = null,
        string? entityType = null,
        Guid? entityId = null)
    {
        HubNotificationTypeId = hubNotificationTypeId;
        NotificationPriorityId = notificationPriorityId;
        Title = title;
        Content = content;
        HubNotificationStatusId = hubNotificationStatusId;
        SenderUserId = senderUserId;
        RecipientUserId = recipientUserId;
        RecipientRoleId = recipientRoleId;
        EntityType = entityType;
        EntityId = entityId;
    }

    public void Update(
        Guid hubNotificationStatusId,
        DateTime? deliveredAt,
        DateTime? readAt)
    {
        HubNotificationStatusId = hubNotificationStatusId;
        DeliveredAt = deliveredAt;
        ReadAt = readAt;
    }
}
