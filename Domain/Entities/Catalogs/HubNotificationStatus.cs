using Domain.Common;

namespace Domain.Entities.Catalogs;

public class HubNotificationStatus : CatalogEntity
{
    private HubNotificationStatus()
    {
    }

    public HubNotificationStatus(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
