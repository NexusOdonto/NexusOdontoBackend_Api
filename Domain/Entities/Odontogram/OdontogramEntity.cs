using Domain.Common;
using Domain.Entities.Clinical;
using Domain.Entities.People;

namespace Domain.Entities.Odontogram;

public class OdontogramEntity : Entity
{
    public Guid ClinicalHistoryId { get; set; }
    public Guid? ClinicalAttentionId { get; set; }
    public Guid ProfessionalId { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public ClinicalHistoryEntity? ClinicalHistoryEntity { get; set; }
    public ClinicalAttentionEntity? ClinicalAttentionEntity { get; set; }
    public ProfessionalEntity? ProfessionalEntity { get; set; }
    public ICollection<OdontogramToothEntity> OdontogramTeeth { get; set; } = [];

    private OdontogramEntity()
    {
    }

    public OdontogramEntity(
        Guid clinicalHistoryId,
        Guid professionalId,
        Guid? clinicalAttentionId = null,
        string? notes = null)
    {
        ClinicalHistoryId = clinicalHistoryId;
        ProfessionalId = professionalId;
        ClinicalAttentionId = clinicalAttentionId;
        Notes = notes;
    }

    public void Update(string? notes, Guid? clinicalAttentionId)
    {
        Notes = notes;
        ClinicalAttentionId = clinicalAttentionId;
    }
}
