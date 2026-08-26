using Domain.Common;

namespace Domain.Entities.Catalogs;

public class AuditEventType : CatalogEntity
{
    public string? Category { get; set; }
    public int? RiskLevel { get; set; }

    private AuditEventType()
    {
    }

    public AuditEventType(
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
