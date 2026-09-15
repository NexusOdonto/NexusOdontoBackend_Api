using Domain.Common;

namespace Domain.Entities.Catalogs;

public class ServiceEntity : CatalogEntity
{
    public int DurationMinutes { get; set; }
    public decimal? Price { get; set; }
    public string? Category { get; set; }

    private ServiceEntity()
    {
    }

    public ServiceEntity(
        string code,
        string name,
        string? description,
        int durationMinutes,
        decimal? price,
        string? category = null,
        int? sortOrder = null,
        bool isActive = true)
        : base(code, name, description, sortOrder)
    {
        DurationMinutes = durationMinutes;
        Price = price;
        Category = category;
        IsActive = isActive;
    }

    public void Update(
        string name,
        string? description,
        int durationMinutes,
        decimal? price,
        string? category,
        int? sortOrder,
        bool isActive)
    {
        base.Update(name, description, sortOrder);
        DurationMinutes = durationMinutes;
        Price = price;
        Category = category;
        SetActive(isActive);
    }
}
