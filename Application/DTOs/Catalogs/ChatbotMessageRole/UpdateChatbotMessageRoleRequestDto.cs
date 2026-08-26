namespace Application.DTOs.Catalogs.ChatbotMessageRole
{
    public class UpdateChatbotMessageRoleRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
    }
}
