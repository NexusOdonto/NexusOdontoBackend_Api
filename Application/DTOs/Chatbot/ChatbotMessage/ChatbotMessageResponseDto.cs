namespace Application.DTOs.Chatbot.ChatbotMessage;

public sealed class ChatbotMessageResponseDto
{
    public Guid Id { get; set; }
    public Guid ChatbotConversationId { get; set; }
    public Guid MessageRoleId { get; set; }
    public string Content { get; set; } = string.Empty;
    public decimal? RagConfidence { get; set; }
    public DateTime CreatedAt { get; set; }
}
