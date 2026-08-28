using Domain.Common;

namespace Domain.Entities.Catalogs;

public class AppointmentStatusEntity : CatalogEntity
{
    public string? ColorHex { get; set; }
    public bool IsFinal { get; set; }

    private AppointmentStatusEntity()
    {
    }

    public AppointmentStatusEntity(
        string code,
        string name,
        string? description = null,
        string? colorHex = null,
        int? sortOrder = null,
        bool isFinal = false)
        : base(code, name, description, sortOrder)
    {
        ColorHex = colorHex;
        IsFinal = isFinal;
    }

    public void Update(
        string name,
        string? description,
        string? colorHex,
        int? sortOrder,
        bool isFinal)
    {
        base.Update(name, description, sortOrder);
        ColorHex = colorHex;
        IsFinal = isFinal;
    }
}
