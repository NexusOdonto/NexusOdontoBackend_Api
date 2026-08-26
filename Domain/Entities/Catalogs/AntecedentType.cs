using Domain.Common;

namespace Domain.Entities.Catalogs;

public class AntecedentType : CatalogEntity
{
    private AntecedentType()
    {
    }

    public AntecedentType(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
