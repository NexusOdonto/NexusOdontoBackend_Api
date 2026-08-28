using Domain.Common;

namespace Domain.Entities.Catalogs;

public class DentitionTypeEntity : CatalogEntity
{
    private DentitionTypeEntity()
    {
    }

    public DentitionTypeEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
