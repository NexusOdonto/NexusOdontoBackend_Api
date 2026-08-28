using Domain.Common;

namespace Domain.Entities.Catalogs;

public class ActionPermissionEntity : CatalogEntity
{
    private ActionPermissionEntity()
    {
    }

    public ActionPermissionEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
