using Domain.Common;

namespace Domain.Entities.Catalogs;

public class Sex : CatalogEntity
{
    private Sex()
    {
    }

    public Sex(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
