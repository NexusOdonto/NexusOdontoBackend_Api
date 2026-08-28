using Domain.Common;

namespace Domain.Entities.Catalogs;

public class ChatbotMessageRoleEntity : CatalogEntity
{
    private ChatbotMessageRoleEntity()
    {
    }

    public ChatbotMessageRoleEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
