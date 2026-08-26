namespace Application.DTOs.Support.SupportTicket
{
    public class UpdateSupportTicketRequestDto
    {
        public Guid TicketReasonId { get; set; }
        public Guid NotificationPriorityId { get; set; }
        public Guid TicketStatusId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public string? Resolution { get; set; }
    }
}
