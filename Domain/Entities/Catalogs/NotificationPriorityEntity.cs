using Domain.Common;

namespace Domain.Entities.Catalogs;

public class NotificationPriorityEntity : CatalogEntity
{
    public string? ColorHex { get; set; }
    public int? Level { get; set; }

    private NotificationPriorityEntity()
    {
    }

    public NotificationPriorityEntity(
        string code,
        string name,
        string? description = null,
        string? colorHex = null,
        int? level = null,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        ColorHex = colorHex;
        Level = level;
    }

    public void Update(
        string name,
        string? description,
        string? colorHex,
        int? level,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        ColorHex = colorHex;
        Level = level;
    }
}
