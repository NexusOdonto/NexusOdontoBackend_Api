using Domain.Common;

namespace Domain.Entities.Catalogs;

public class SessionStatusEntity : CatalogEntity
{
    private SessionStatusEntity()
    {
    }

    public SessionStatusEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
