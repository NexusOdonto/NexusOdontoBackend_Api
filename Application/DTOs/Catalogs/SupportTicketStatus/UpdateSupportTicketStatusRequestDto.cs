namespace Application.DTOs.Catalogs.SupportTicketStatus
{
    public class UpdateSupportTicketStatusRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsFinal { get; set; }
        public bool AllowsAssignment { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
    }
}
