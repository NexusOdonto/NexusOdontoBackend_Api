using Domain.Common;

namespace Domain.Entities.Catalogs;

public class SupportTicketReasonEntity : CatalogEntity
{
    private SupportTicketReasonEntity()
    {
    }

    public SupportTicketReasonEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
