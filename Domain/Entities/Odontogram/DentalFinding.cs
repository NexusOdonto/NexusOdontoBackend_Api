using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.Odontogram;

public class DentalFinding : Entity
{
    public Guid OdontogramToothId { get; set; }
    public Guid? DentalSurfaceId { get; set; }
    public Guid FindingTypeId { get; set; }
    public Guid FindingStatusId { get; set; }
    public string? Note { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public OdontogramTooth? OdontogramTooth { get; set; }
    public DentalSurface? DentalSurface { get; set; }
    public FindingType? FindingType { get; set; }
    public FindingStatus? FindingStatus { get; set; }

    private DentalFinding()
    {
    }

    public DentalFinding(
        Guid odontogramToothId,
        Guid findingTypeId,
        Guid findingStatusId,
        Guid? dentalSurfaceId = null,
        string? note = null)
    {
        OdontogramToothId = odontogramToothId;
        FindingTypeId = findingTypeId;
        FindingStatusId = findingStatusId;
        DentalSurfaceId = dentalSurfaceId;
        Note = note;
    }

    public void Update(
        Guid? dentalSurfaceId,
        Guid findingTypeId,
        Guid findingStatusId,
        string? note)
    {
        DentalSurfaceId = dentalSurfaceId;
        FindingTypeId = findingTypeId;
        FindingStatusId = findingStatusId;
        Note = note;
    }
}
