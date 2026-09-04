namespace Application.DTOs.Chatbot.ChatbotConversation;

public class UpdateChatbotConversationRequestDto
{
    public Guid ConversationStatusId { get; set; }
    public Guid? PatientId { get; set; }
    public DateTime? ClosedAt { get; set; }
}
