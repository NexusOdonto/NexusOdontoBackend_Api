namespace Application.DTOs.Chatbot.ChatbotConversation;

public class UpdateChatbotConversationRequestDto
{
    public Guid ConversationStatusId { get; set; }
    public Guid? PatientId { get; set; }
    public DateTime? ClosedAt { get; set; }
    /// <summary>Optional WhatsApp origin phone / JID repair (E.164 preferred).</summary>
    public string? ChatIdentifier { get; set; }
}
