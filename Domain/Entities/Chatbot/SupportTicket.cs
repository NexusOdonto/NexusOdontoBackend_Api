using Domain.Common;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Domain.Entities.Chatbot;

public class SupportTicket : Entity
{
    public Guid ChatbotConversationId { get; set; }
    public Guid? PatientId { get; set; }
    public Guid TicketReasonId { get; set; }
    public Guid NotificationPriorityId { get; set; }
    public Guid TicketStatusId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ChatbotSummary { get; set; }
    public decimal? RagConfidence { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? AssignedToUserId { get; set; }
    public DateTime? AssignedAt { get; set; }
    public DateTime? AttendedAt { get; set; }
    public Guid? ResolvedByUserId { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? Resolution { get; set; }
    public DateTime? ClosedAt { get; set; }

    public ChatbotConversation? ChatbotConversation { get; set; }
    public Patient? Patient { get; set; }
    public SupportTicketReason? TicketReason { get; set; }
    public NotificationPriority? NotificationPriority { get; set; }
    public SupportTicketStatus? TicketStatus { get; set; }
    public User? AssignedToUser { get; set; }
    public User? ResolvedByUser { get; set; }

    private SupportTicket()
    {
    }

    public SupportTicket(
        Guid chatbotConversationId,
        Guid ticketReasonId,
        Guid notificationPriorityId,
        Guid ticketStatusId,
        string title,
        Guid? patientId = null,
        string? description = null,
        string? chatbotSummary = null,
        decimal? ragConfidence = null)
    {
        ChatbotConversationId = chatbotConversationId;
        TicketReasonId = ticketReasonId;
        NotificationPriorityId = notificationPriorityId;
        TicketStatusId = ticketStatusId;
        Title = title;
        PatientId = patientId;
        Description = description;
        ChatbotSummary = chatbotSummary;
        RagConfidence = ragConfidence;
    }

    public void Update(
        Guid ticketStatusId,
        Guid notificationPriorityId,
        Guid? assignedToUserId,
        DateTime? assignedAt,
        DateTime? attendedAt,
        Guid? resolvedByUserId,
        DateTime? resolvedAt,
        string? resolution,
        DateTime? closedAt)
    {
        TicketStatusId = ticketStatusId;
        NotificationPriorityId = notificationPriorityId;
        AssignedToUserId = assignedToUserId;
        AssignedAt = assignedAt;
        AttendedAt = attendedAt;
        ResolvedByUserId = resolvedByUserId;
        ResolvedAt = resolvedAt;
        Resolution = resolution;
        ClosedAt = closedAt;
    }
}
