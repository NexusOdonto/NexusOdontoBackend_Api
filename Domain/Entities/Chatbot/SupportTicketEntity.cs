using Domain.Common;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Domain.Entities.Chatbot;

public class SupportTicketEntity : Entity
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

    public ChatbotConversationEntity? ChatbotConversationEntity { get; set; }
    public PatientEntity? PatientEntity { get; set; }
    public SupportTicketReasonEntity? TicketReason { get; set; }
    public NotificationPriorityEntity? NotificationPriorityEntity { get; set; }
    public SupportTicketStatusEntity? TicketStatus { get; set; }
    public UserEntity? AssignedToUser { get; set; }
    public UserEntity? ResolvedByUser { get; set; }

    private SupportTicketEntity()
    {
    }

    public SupportTicketEntity(
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
