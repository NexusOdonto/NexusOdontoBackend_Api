namespace Application.DTOs.Catalogs.ChatChannel
{
    public class UpdateChatChannelRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
    }
}
