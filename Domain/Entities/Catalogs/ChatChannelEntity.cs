using Domain.Common;

namespace Domain.Entities.Catalogs;

public class ChatChannelEntity : CatalogEntity
{
    private ChatChannelEntity()
    {
    }

    public ChatChannelEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
