using Domain.Entities.Chatbot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Chatbot;

public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        builder.ToTable("support_tickets");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ChatbotConversationId)
            .HasColumnName("chatbot_conversation_id")
            .IsRequired();

        builder.Property(c => c.PatientId)
            .HasColumnName("patient_id");

        builder.Property(c => c.TicketReasonId)
            .HasColumnName("support_ticket_reason_id")
            .IsRequired();

        builder.Property(c => c.NotificationPriorityId)
            .HasColumnName("notification_priority_id")
            .IsRequired();

        builder.Property(c => c.TicketStatusId)
            .HasColumnName("ticket_status_id")
            .IsRequired();

        builder.Property(c => c.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnName("description");

        builder.Property(c => c.ChatbotSummary)
            .HasColumnName("chatbot_summary");

        builder.Property(c => c.RagConfidence)
            .HasColumnName("rag_confidence")
            .HasPrecision(5, 4);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.AssignedToUserId)
            .HasColumnName("assigned_to_user_id");

        builder.Property(c => c.AssignedAt)
            .HasColumnName("assigned_at");

        builder.Property(c => c.AttendedAt)
            .HasColumnName("attended_at");

        builder.Property(c => c.ResolvedByUserId)
            .HasColumnName("resolved_by_user_id");

        builder.Property(c => c.ResolvedAt)
            .HasColumnName("resolved_at");

        builder.Property(c => c.Resolution)
            .HasColumnName("resolution");

        builder.Property(c => c.ClosedAt)
            .HasColumnName("closed_at");
    }
}
