using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.Odontogram;

public class OdontogramTooth : Entity
{
    public Guid OdontogramId { get; set; }
    public string ToothCode { get; set; } = null!;
    public Guid ToothStatusId { get; set; }
    public string? Note { get; set; }

    public Odontogram? Odontogram { get; set; }
    public Tooth? Tooth { get; set; }
    public ToothStatus? ToothStatus { get; set; }
    public ICollection<DentalFinding> DentalFindings { get; set; } = [];

    private OdontogramTooth()
    {
    }

    public OdontogramTooth(
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
