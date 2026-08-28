using Domain.Common;

namespace Domain.Entities.Catalogs;

public class AntecedentTypeEntity : CatalogEntity
{
    private AntecedentTypeEntity()
    {
    }

    public AntecedentTypeEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
