using Domain.Common;

namespace Domain.Entities.Catalogs;

public class JobTitle : CatalogEntity
{
    private JobTitle()
    {
    }

    public JobTitle(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
