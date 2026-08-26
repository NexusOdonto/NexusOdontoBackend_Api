namespace Domain.Common;

public abstract class CatalogEntity : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? SortOrder { get; set; }

    protected CatalogEntity()
    {
    }

    protected CatalogEntity(
        string code,
        string name,
        string? description = null,
        int? sortOrder = null)
    {
        Code = code;
        Name = name;
        Description = description;
        SortOrder = sortOrder;
    }

    public virtual void Update(string name, string? description, int? sortOrder)
    {
        UpdatedAt = DateTime.UtcNow;
        Name = name;
        Description = description;
        SortOrder = sortOrder;
    }
}
