using Domain.Common;

namespace Domain.Entities.Catalogs;

public class SupportTicketStatusEntity : CatalogEntity
{
    public bool IsFinal { get; set; }
    public bool AllowsAssignment { get; set; }

    private SupportTicketStatusEntity()
    {
    }

    public SupportTicketStatusEntity(
        string code,
        string name,
        string? description = null,
        bool isFinal = false,
        bool allowsAssignment = false,
        int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
        IsFinal = isFinal;
        AllowsAssignment = allowsAssignment;
    }

    public void Update(
        string name,
        string? description,
        bool isFinal,
        bool allowsAssignment,
        int? sortOrder)
    {
        base.Update(name, description, sortOrder);
        IsFinal = isFinal;
        AllowsAssignment = allowsAssignment;
    }
}
