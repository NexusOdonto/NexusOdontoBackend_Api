using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.Chatbot;

public class ChatbotMessageEntity : Entity
{
    public Guid ChatbotConversationId { get; set; }
    public Guid MessageRoleId { get; set; }
    public string Content { get; set; } = null!;
    public decimal? RagConfidence { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ChatbotConversationEntity? ChatbotConversationEntity { get; set; }
    public ChatbotMessageRoleEntity? MessageRole { get; set; }

    private ChatbotMessageEntity()
    {
    }

    public ChatbotMessageEntity(
        Guid chatbotConversationId,
        Guid messageRoleId,
        string content,
        decimal? ragConfidence = null)
    {
        ChatbotConversationId = chatbotConversationId;
        MessageRoleId = messageRoleId;
        Content = content;
        RagConfidence = ragConfidence;
    }
}
