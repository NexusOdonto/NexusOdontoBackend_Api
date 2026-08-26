using Domain.Entities.Chatbot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Chatbot;

public class ChatbotMessageConfiguration : IEntityTypeConfiguration<ChatbotMessage>
{
    public void Configure(EntityTypeBuilder<ChatbotMessage> builder)
    {
        builder.ToTable("chatbot_messages");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ChatbotConversationId)
            .HasColumnName("chatbot_conversation_id")
            .IsRequired();

        builder.Property(c => c.MessageRoleId)
            .HasColumnName("message_role_id")
            .IsRequired();

        builder.Property(c => c.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(c => c.RagConfidence)
            .HasColumnName("rag_confidence")
            .HasPrecision(5, 4);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
    }
}
