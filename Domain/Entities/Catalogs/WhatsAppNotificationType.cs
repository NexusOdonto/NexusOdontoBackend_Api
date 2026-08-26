using Domain.Common;

namespace Domain.Entities.Catalogs;

public class WhatsAppNotificationType : CatalogEntity
{
    public string? DefaultTemplate { get; set; }

    private WhatsAppNotificationType()
    {
    }

    public WhatsAppNotificationType(
        string code,
        string name,
        string? description = null,
        string? defaultTemplate = null,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        DefaultTemplate = defaultTemplate;
    }

    public void Update(
        string name,
        string? description,
        string? defaultTemplate,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        DefaultTemplate = defaultTemplate;
    }
}
