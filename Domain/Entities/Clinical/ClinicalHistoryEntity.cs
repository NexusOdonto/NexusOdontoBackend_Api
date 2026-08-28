using Domain.Common;
using Domain.Entities.People;

namespace Domain.Entities.Clinical;

public class ClinicalHistoryEntity : Entity
{
    public Guid PatientId { get; set; }
    public DateTime OpenedAt { get; set; } = DateTime.UtcNow.Date;
    public bool IsActive { get; set; } = true;
    public string? GeneralNotes { get; set; }

    public PatientEntity? PatientEntity { get; set; }

    private ClinicalHistoryEntity()
    {
    }

    public ClinicalHistoryEntity(Guid patientId, DateTime? openedAt = null, string? generalNotes = null)
    {
        PatientId = patientId;
        OpenedAt = (openedAt ?? DateTime.UtcNow).Date;
        GeneralNotes = generalNotes;
    }

    public void Update(bool isActive, string? generalNotes)
    {
        IsActive = isActive;
        GeneralNotes = generalNotes;
    }
}
