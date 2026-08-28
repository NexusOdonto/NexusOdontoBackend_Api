using Domain.Common;

namespace Domain.Entities.Catalogs;

public class SexEntity : CatalogEntity
{
    private SexEntity()
    {
    }

    public SexEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
