using Domain.Common;

namespace Domain.Entities.Catalogs;

public class SessionStatus : CatalogEntity
{
    private SessionStatus()
    {
    }

    public SessionStatus(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
