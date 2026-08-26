using Domain.Entities.Chatbot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Chatbot;

public class ChatbotConversationConfiguration : IEntityTypeConfiguration<ChatbotConversation>
{
    public void Configure(EntityTypeBuilder<ChatbotConversation> builder)
    {
        builder.ToTable("chatbot_conversations");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.PatientId)
            .HasColumnName("patient_id");

        builder.Property(c => c.ChatIdentifier)
            .HasColumnName("chat_identifier")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.ChatChannelId)
            .HasColumnName("chat_channel_id")
            .IsRequired();

        builder.Property(c => c.ConversationStatusId)
            .HasColumnName("conversation_status_id")
            .IsRequired();

        builder.Property(c => c.StartedAt)
            .HasColumnName("started_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.LastInteractionAt)
            .HasColumnName("last_interaction_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.ClosedAt)
            .HasColumnName("closed_at");

        builder.HasIndex(c => new { c.ChatChannelId, c.ChatIdentifier }).IsUnique();
    }
}
