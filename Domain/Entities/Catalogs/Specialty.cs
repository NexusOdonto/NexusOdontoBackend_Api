using Domain.Common;
using Domain.Entities.People;

namespace Domain.Entities.Catalogs;

public class Specialty : CatalogEntity
{
    public ICollection<Professional> Professionals { get; set; } = [];

    private Specialty()
    {
    }

    public Specialty(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
