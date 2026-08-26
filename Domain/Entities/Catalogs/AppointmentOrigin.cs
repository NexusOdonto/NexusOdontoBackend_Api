using Domain.Common;

namespace Domain.Entities.Catalogs;

public class AppointmentOrigin : CatalogEntity
{
    private AppointmentOrigin()
    {
    }

    public AppointmentOrigin(string code, string name, string? description = null, int? sortOrder = null)
        : base(code, name, description, sortOrder)
    {
    }
}
