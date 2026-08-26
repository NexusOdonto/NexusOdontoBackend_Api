namespace Application.DTOs.Chatbot.ChatbotMessage
{
    public class CreateChatbotMessageRequestDto
    {
        public Guid ChatbotConversationId { get; set; }
        public Guid MessageRoleId { get; set; }
        public string Content { get; set; } = string.Empty;
        public decimal? RagConfidence { get; set; }
    }
}
