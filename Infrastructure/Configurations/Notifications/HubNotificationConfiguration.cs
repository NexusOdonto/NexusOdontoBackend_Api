using Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Notifications;

public class HubNotificationConfiguration : IEntityTypeConfiguration<HubNotification>
{
    public void Configure(EntityTypeBuilder<HubNotification> builder)
    {
        builder.ToTable("hub_notifications");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.SenderUserId)
            .HasColumnName("sender_user_id");

        builder.Property(c => c.RecipientUserId)
            .HasColumnName("recipient_user_id");

        builder.Property(c => c.RecipientRoleId)
            .HasColumnName("recipient_role_id");

        builder.Property(c => c.HubNotificationTypeId)
            .HasColumnName("hub_notification_type_id")
            .IsRequired();

        builder.Property(c => c.NotificationPriorityId)
            .HasColumnName("notification_priority_id")
            .IsRequired();

        builder.Property(c => c.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(c => c.EntityType)
            .HasColumnName("entity_type")
            .HasMaxLength(80);

        builder.Property(c => c.EntityId)
            .HasColumnName("entity_id");

        builder.Property(c => c.HubNotificationStatusId)
            .HasColumnName("hub_notification_status_id")
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.DeliveredAt)
            .HasColumnName("delivered_at");

        builder.Property(c => c.ReadAt)
            .HasColumnName("read_at");
    }
}
