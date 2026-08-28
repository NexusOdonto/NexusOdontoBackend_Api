namespace Application.DTOs.Support.SupportTicket;

public sealed class SupportTicketResponseDto
{
    public Guid Id { get; set; }
    public Guid? ChatbotConversationId { get; set; }
    public Guid? PatientId { get; set; }
    public Guid TicketReasonId { get; set; }
    public Guid NotificationPriorityId { get; set; }
    public Guid TicketStatusId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ChatbotSummary { get; set; }
    public decimal? RagConfidence { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public DateTime? AssignedAt { get; set; }
    public string? Resolution { get; set; }
    public DateTime? ClosedAt { get; set; }
}
