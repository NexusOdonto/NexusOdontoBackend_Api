using Domain.Common;

namespace Domain.Entities.Catalogs;

public class JobTitleEntity : CatalogEntity
{
    private JobTitleEntity()
    {
    }

    public JobTitleEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
