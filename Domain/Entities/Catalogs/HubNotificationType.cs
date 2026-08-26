using Domain.Common;

namespace Domain.Entities.Catalogs;

public class HubNotificationType : CatalogEntity
{
    public string? Icon { get; set; }

    private HubNotificationType()
    {
    }

    public HubNotificationType(
        string code,
        string name,
        string? description = null,
        string? icon = null,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        Icon = icon;
    }

    public void Update(
        string name,
        string? description,
        string? icon,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        Icon = icon;
    }
}
