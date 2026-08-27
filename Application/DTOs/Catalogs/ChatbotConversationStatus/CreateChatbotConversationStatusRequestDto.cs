namespace Application.DTOs.Catalogs.ChatbotConversationStatus
{
    public class CreateChatbotConversationStatusRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsFinal { get; set; }
        public int? SortOrder { get; set; }
    }
}
