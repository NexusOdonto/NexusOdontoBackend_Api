using Domain.Common;

namespace Domain.Entities.Catalogs;

public class DentitionType : CatalogEntity
{
    private DentitionType()
    {
    }

    public DentitionType(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
