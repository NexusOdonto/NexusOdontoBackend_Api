namespace Application.DTOs.Catalogs.ChatChannel
{
    public class CreateChatChannelRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
    }
}
