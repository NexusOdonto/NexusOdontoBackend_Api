using Domain.Common;

namespace Domain.Entities.Catalogs;

public class WhatsAppNotificationStatus : CatalogEntity
{
    private WhatsAppNotificationStatus()
    {
    }

    public WhatsAppNotificationStatus(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
