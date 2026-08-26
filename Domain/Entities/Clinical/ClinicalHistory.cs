using Domain.Common;
using Domain.Entities.People;

namespace Domain.Entities.Clinical;

public class ClinicalHistory : Entity
{
    public Guid PatientId { get; set; }
    public DateTime OpenedAt { get; set; } = DateTime.UtcNow.Date;
    public bool IsActive { get; set; } = true;
    public string? GeneralNotes { get; set; }

    public Patient? Patient { get; set; }

    private ClinicalHistory()
    {
    }

    public ClinicalHistory(Guid patientId, DateTime? openedAt = null, string? generalNotes = null)
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
