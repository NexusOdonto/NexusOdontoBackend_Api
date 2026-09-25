using Domain.Common;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Domain.Entities.Chatbot;

public class ChatbotConversationEntity : Entity
{
    public Guid? PatientId { get; set; }
    public string ChatIdentifier { get; set; } = null!;
    public Guid ChatChannelId { get; set; }
    public Guid ConversationStatusId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastInteractionAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }

    public PatientEntity? PatientEntity { get; set; }
    public ChatChannelEntity? ChatChannelEntity { get; set; }
    public ChatbotConversationStatusEntity? ConversationStatus { get; set; }
    public ICollection<ChatbotMessageEntity> Messages { get; set; } = [];

    private ChatbotConversationEntity()
    {
    }

    public ChatbotConversationEntity(
        string chatIdentifier,
        Guid chatChannelId,
        Guid conversationStatusId,
        Guid? patientId = null)
    {
        ChatIdentifier = chatIdentifier;
        ChatChannelId = chatChannelId;
        ConversationStatusId = conversationStatusId;
        PatientId = patientId;
    }

    public void Update(
        Guid conversationStatusId,
        DateTime lastInteractionAt,
        DateTime? closedAt,
        Guid? patientId)
    {
        ConversationStatusId = conversationStatusId;
        LastInteractionAt = lastInteractionAt;
        ClosedAt = closedAt;
        PatientId = patientId;
    }

    public void SetChatIdentifier(string chatIdentifier)
    {
        if (string.IsNullOrWhiteSpace(chatIdentifier))
            return;
        ChatIdentifier = chatIdentifier.Trim();
    }
}
