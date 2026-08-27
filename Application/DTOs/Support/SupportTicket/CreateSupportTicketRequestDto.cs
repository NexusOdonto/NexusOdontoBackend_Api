namespace Application.DTOs.Support.SupportTicket
{
    public class CreateSupportTicketRequestDto
    {
        public Guid ChatbotConversationId { get; set; }
        public Guid? PatientId { get; set; }
        public Guid TicketReasonId { get; set; }
        public Guid NotificationPriorityId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ChatbotSummary { get; set; }
        public decimal? RagConfidence { get; set; }
    }
}
