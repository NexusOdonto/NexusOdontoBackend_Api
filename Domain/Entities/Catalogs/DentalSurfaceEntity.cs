using Domain.Common;

namespace Domain.Entities.Catalogs;

public class DentalSurfaceEntity : CatalogEntity
{
    public bool AppliesToAnterior { get; set; } = true;
    public bool AppliesToPosterior { get; set; } = true;

    private DentalSurfaceEntity()
    {
    }

    public DentalSurfaceEntity(
        string code,
        string name,
        string? description = null,
        bool appliesToAnterior = true,
        bool appliesToPosterior = true,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        AppliesToAnterior = appliesToAnterior;
        AppliesToPosterior = appliesToPosterior;
    }

    public void Update(
        string name,
        string? description,
        bool appliesToAnterior,
        bool appliesToPosterior,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        AppliesToAnterior = appliesToAnterior;
        AppliesToPosterior = appliesToPosterior;
    }
}
