using Domain.Common;

namespace Domain.Entities.Catalogs;

public class DocumentType : CatalogEntity
{
    private DocumentType()
    {
    }

    public DocumentType(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
