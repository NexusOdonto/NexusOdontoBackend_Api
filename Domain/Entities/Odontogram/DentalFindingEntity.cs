using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.Odontogram;

public class DentalFindingEntity : Entity
{
    public Guid OdontogramToothId { get; set; }
    public Guid? DentalSurfaceId { get; set; }
    public Guid FindingTypeId { get; set; }
    public Guid FindingStatusId { get; set; }
    public string? Note { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public OdontogramToothEntity? OdontogramToothEntity { get; set; }
    public DentalSurfaceEntity? DentalSurfaceEntity { get; set; }
    public FindingTypeEntity? FindingTypeEntity { get; set; }
    public FindingStatusEntity? FindingStatusEntity { get; set; }

    private DentalFindingEntity()
    {
    }

    public DentalFindingEntity(
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
