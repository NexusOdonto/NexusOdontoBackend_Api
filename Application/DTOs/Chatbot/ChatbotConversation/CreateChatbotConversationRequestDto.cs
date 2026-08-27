namespace Application.DTOs.Chatbot.ChatbotConversation
{
    public class CreateChatbotConversationRequestDto
    {
        public Guid? PatientId { get; set; }
        public string ChatIdentifier { get; set; } = string.Empty;
        public Guid ChatChannelId { get; set; }
    }
}
