namespace Application.DTOs.Catalogs.SupportTicketReason
{
    public class CreateSupportTicketReasonRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
    }
}
