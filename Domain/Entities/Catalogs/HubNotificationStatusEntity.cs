using Domain.Common;

namespace Domain.Entities.Catalogs;

public class HubNotificationStatusEntity : CatalogEntity
{
    private HubNotificationStatusEntity()
    {
    }

    public HubNotificationStatusEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
