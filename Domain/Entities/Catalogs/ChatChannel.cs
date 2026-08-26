using Domain.Common;

namespace Domain.Entities.Catalogs;

public class ChatChannel : CatalogEntity
{
    private ChatChannel()
    {
    }

    public ChatChannel(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
