using Domain.Common;

namespace Domain.Entities.Catalogs;

public class ActionPermission : CatalogEntity
{
    private ActionPermission()
    {
    }

    public ActionPermission(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
