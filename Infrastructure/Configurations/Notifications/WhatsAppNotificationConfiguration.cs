using Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Notifications;

public class WhatsAppNotificationConfiguration : IEntityTypeConfiguration<WhatsAppNotificationEntity>
{
    public void Configure(EntityTypeBuilder<WhatsAppNotificationEntity> builder)
    {
        builder.ToTable("whatsapp_notifications");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.AppointmentId)
            .HasColumnName("appointment_id");

        builder.Property(c => c.PatientId)
            .HasColumnName("patient_id");

        builder.Property(c => c.WhatsAppNotificationTypeId)
            .HasColumnName("whatsapp_notification_type_id")
            .IsRequired();

        builder.Property(c => c.WhatsAppNotificationStatusId)
            .HasColumnName("whatsapp_notification_status_id")
            .IsRequired();

        builder.Property(c => c.DestinationPhone)
            .HasColumnName("destination_phone")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(c => c.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(c => c.TemplateName)
            .HasColumnName("template_name")
            .HasMaxLength(100);

        builder.Property(c => c.TemplateParameters)
            .HasColumnName("template_parameters");

        builder.Property(c => c.Provider)
            .HasColumnName("provider")
            .HasMaxLength(50);

        builder.Property(c => c.ProviderMessageId)
            .HasColumnName("provider_message_id")
            .HasMaxLength(150);

        builder.Property(c => c.ScheduledFor)
            .HasColumnName("scheduled_for")
            .IsRequired();

        builder.Property(c => c.SentAt)
            .HasColumnName("sent_at");

        builder.Property(c => c.DeliveredAt)
            .HasColumnName("delivered_at");

        builder.Property(c => c.ReadAt)
            .HasColumnName("read_at");

        builder.Property(c => c.FailedAt)
            .HasColumnName("failed_at");

        builder.Property(c => c.ProviderResponse)
            .HasColumnName("provider_response");

        builder.Property(c => c.ErrorDetail)
            .HasColumnName("error_detail")
            .HasMaxLength(1000);
    }
}
