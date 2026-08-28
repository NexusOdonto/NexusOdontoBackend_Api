using Domain.Common;

namespace Domain.Entities.Catalogs;

public class DocumentTypeEntity : CatalogEntity
{
    private DocumentTypeEntity()
    {
    }

    public DocumentTypeEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
