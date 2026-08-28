using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.Odontogram;

public class OdontogramToothEntity : Entity
{
    public Guid OdontogramId { get; set; }
    public string ToothCode { get; set; } = null!;
    public Guid ToothStatusId { get; set; }
    public string? Note { get; set; }

    public OdontogramEntity? OdontogramEntity { get; set; }
    public ToothEntity? ToothEntity { get; set; }
    public ToothStatusEntity? ToothStatusEntity { get; set; }
    public ICollection<DentalFindingEntity> DentalFindings { get; set; } = [];

    private OdontogramToothEntity()
    {
    }

    public OdontogramToothEntity(
        Guid odontogramId,
        string toothCode,
        Guid toothStatusId,
        string? note = null)
    {
        OdontogramId = odontogramId;
        ToothCode = toothCode;
        ToothStatusId = toothStatusId;
        Note = note;
    }

    public void Update(Guid toothStatusId, string? note)
    {
        ToothStatusId = toothStatusId;
        Note = note;
    }
}
