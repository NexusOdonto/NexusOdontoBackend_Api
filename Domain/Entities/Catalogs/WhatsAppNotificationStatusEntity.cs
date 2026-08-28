using Domain.Common;

namespace Domain.Entities.Catalogs;

public class WhatsAppNotificationStatusEntity : CatalogEntity
{
    private WhatsAppNotificationStatusEntity()
    {
    }

    public WhatsAppNotificationStatusEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
