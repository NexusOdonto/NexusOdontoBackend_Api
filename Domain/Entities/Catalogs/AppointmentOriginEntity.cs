using Domain.Common;

namespace Domain.Entities.Catalogs;

public class AppointmentOriginEntity : CatalogEntity
{
    private AppointmentOriginEntity()
    {
    }

    public AppointmentOriginEntity(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
