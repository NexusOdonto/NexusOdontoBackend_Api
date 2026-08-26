using Domain.Common;
using Domain.Entities.Clinical;
using Domain.Entities.People;

namespace Domain.Entities.Odontogram;

public class Odontogram : Entity
{
    public Guid ClinicalHistoryId { get; set; }
    public Guid? ClinicalAttentionId { get; set; }
    public Guid ProfessionalId { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public ClinicalHistory? ClinicalHistory { get; set; }
    public ClinicalAttention? ClinicalAttention { get; set; }
    public Professional? Professional { get; set; }
    public ICollection<OdontogramTooth> OdontogramTeeth { get; set; } = [];

    private Odontogram()
    {
    }

    public Odontogram(
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
