namespace Application.DTOs.Chatbot.ChatbotConversation;

public sealed class ChatbotConversationResponseDto
{
    public Guid Id { get; set; }
    public Guid? PatientId { get; set; }
    public string ChatIdentifier { get; set; } = string.Empty;
    public Guid ChatChannelId { get; set; }
    public Guid ConversationStatusId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? LastInteractionAt { get; set; }
    public DateTime? ClosedAt { get; set; }
}
