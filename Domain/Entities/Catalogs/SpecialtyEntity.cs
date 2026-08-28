using Domain.Common;
using Domain.Entities.People;

namespace Domain.Entities.Catalogs;

public class SpecialtyEntity : CatalogEntity
{
    public ICollection<ProfessionalEntity> Professionals { get; set; } = [];

    private SpecialtyEntity()
    {
    }

    public SpecialtyEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
