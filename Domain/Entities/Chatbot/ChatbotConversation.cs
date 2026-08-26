using Domain.Common;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Domain.Entities.Chatbot;

public class ChatbotConversation : Entity
{
    public Guid? PatientId { get; set; }
    public string ChatIdentifier { get; set; } = null!;
    public Guid ChatChannelId { get; set; }
    public Guid ConversationStatusId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastInteractionAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }

    public Patient? Patient { get; set; }
    public ChatChannel? ChatChannel { get; set; }
    public ChatbotConversationStatus? ConversationStatus { get; set; }
    public ICollection<ChatbotMessage> Messages { get; set; } = [];

    private ChatbotConversation()
    {
    }

    public ChatbotConversation(
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
}
