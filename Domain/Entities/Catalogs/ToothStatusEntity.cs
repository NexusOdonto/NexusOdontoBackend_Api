using Domain.Common;

namespace Domain.Entities.Catalogs;

public class ToothStatusEntity : CatalogEntity
{
    public string? ColorHex { get; set; }
    public string? VisualCode { get; set; }

    private ToothStatusEntity()
    {
    }

    public ToothStatusEntity(
        string code,
        string name,
        string? description = null,
        string? colorHex = null,
        string? visualCode = null,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        ColorHex = colorHex;
        VisualCode = visualCode;
    }

    public void Update(
        string name,
        string? description,
        string? colorHex,
        string? visualCode,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        ColorHex = colorHex;
        VisualCode = visualCode;
    }
}
