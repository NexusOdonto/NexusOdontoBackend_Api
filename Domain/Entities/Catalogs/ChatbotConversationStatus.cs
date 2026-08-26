using Domain.Common;

namespace Domain.Entities.Catalogs;

public class ChatbotConversationStatus : CatalogEntity
{
    public bool IsFinal { get; set; }

    private ChatbotConversationStatus()
    {
    }

    public ChatbotConversationStatus(
        string code,
        string name,
        string? description = null,
        bool isFinal = false,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        IsFinal = isFinal;
    }

    public void Update(
        string name,
        string? description,
        bool isFinal,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        IsFinal = isFinal;
    }
}
