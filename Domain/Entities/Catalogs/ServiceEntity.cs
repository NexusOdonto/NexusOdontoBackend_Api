using Domain.Common;

namespace Domain.Entities.Catalogs;

public class ServiceEntity : CatalogEntity
{
    public int DurationMinutes { get; set; }
    public decimal? Price { get; set; }

    private ServiceEntity()
    {
    }

    public ServiceEntity(
        string code,
        string name,
        string? description,
        int durationMinutes,
        decimal? price,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        DurationMinutes = durationMinutes;
        Price = price;
    }

    public void Update(
        string name,
        string? description,
        int durationMinutes,
        decimal? price,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        DurationMinutes = durationMinutes;
        Price = price;
    }
}
