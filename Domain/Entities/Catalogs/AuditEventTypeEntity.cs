using Domain.Common;

namespace Domain.Entities.Catalogs;

public class AuditEventTypeEntity : CatalogEntity
{
    public string? Category { get; set; }
    public int? RiskLevel { get; set; }

    private AuditEventTypeEntity()
    {
    }

    public AuditEventTypeEntity(
        string code,
        string name,
        string? description = null,
        string? category = null,
        int? riskLevel = null,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        Category = category;
        RiskLevel = riskLevel;
    }

    public void Update(
        string name,
        string? description,
        string? category,
        int? riskLevel,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        Category = category;
        RiskLevel = riskLevel;
    }
}
