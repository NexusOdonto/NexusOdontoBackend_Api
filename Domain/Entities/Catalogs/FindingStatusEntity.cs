using Domain.Common;

namespace Domain.Entities.Catalogs;

public class FindingStatusEntity : CatalogEntity
{
    public string? ColorHex { get; set; }

    private FindingStatusEntity()
    {
    }

    public FindingStatusEntity(
        string code,
        string name,
        string? description = null,
        string? colorHex = null,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        ColorHex = colorHex;
    }

    public void Update(
        string name,
        string? description,
        string? colorHex,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        ColorHex = colorHex;
    }
}
