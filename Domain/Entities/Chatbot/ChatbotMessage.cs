using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.Chatbot;

public class ChatbotMessage : Entity
{
    public Guid ChatbotConversationId { get; set; }
    public Guid MessageRoleId { get; set; }
    public string Content { get; set; } = null!;
    public decimal? RagConfidence { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ChatbotConversation? ChatbotConversation { get; set; }
    public ChatbotMessageRole? MessageRole { get; set; }

    private ChatbotMessage()
    {
    }

    public ChatbotMessage(
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
