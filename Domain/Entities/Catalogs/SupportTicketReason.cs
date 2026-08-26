using Domain.Common;

namespace Domain.Entities.Catalogs;

public class SupportTicketReason : CatalogEntity
{
    private SupportTicketReason()
    {
    }

    public SupportTicketReason(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
