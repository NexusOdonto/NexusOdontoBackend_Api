using Domain.Common;

namespace Domain.Entities.Catalogs;

public class FindingType : CatalogEntity
{
    public string? ColorHex { get; set; }
    public string? VisualSymbol { get; set; }
    public bool RequiresSurface { get; set; }

    private FindingType()
    {
    }

    public FindingType(
        string code,
        string name,
        string? description = null,
        string? colorHex = null,
        string? visualSymbol = null,
        bool requiresSurface = false,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        ColorHex = colorHex;
        VisualSymbol = visualSymbol;
        RequiresSurface = requiresSurface;
    }

    public void Update(
        string name,
        string? description,
        string? colorHex,
        string? visualSymbol,
        bool requiresSurface,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        ColorHex = colorHex;
        VisualSymbol = visualSymbol;
        RequiresSurface = requiresSurface;
    }
}
