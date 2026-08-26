using Domain.Common;

namespace Domain.Entities.Catalogs;

public class ChatbotMessageRole : CatalogEntity
{
    private ChatbotMessageRole()
    {
    }

    public ChatbotMessageRole(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
