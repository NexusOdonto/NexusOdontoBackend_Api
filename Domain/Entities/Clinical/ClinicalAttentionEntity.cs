using Domain.Common;
using Domain.Entities.Schedule;

namespace Domain.Entities.Clinical;

public class ClinicalAttentionEntity : Entity
{
    public Guid AppointmentId { get; set; }
    public DateTime AttentionAt { get; set; } = DateTime.UtcNow;
    public string? ClinicalExam { get; set; }
    public string? ProgressNotes { get; set; }
    public string? TreatmentPlan { get; set; }

    public AppointmentEntity? AppointmentEntity { get; set; }
    public ICollection<DiagnosisEntity> Diagnoses { get; set; } = [];
    public ICollection<ProcedurePerformedEntity> ProceduresPerformed { get; set; } = [];

    private ClinicalAttentionEntity()
    {
    }

    public ClinicalAttentionEntity(
        Guid appointmentId,
        DateTime? attentionAt = null,
        string? clinicalExam = null,
        string? progressNotes = null,
        string? treatmentPlan = null)
    {
        AppointmentId = appointmentId;
        AttentionAt = attentionAt ?? DateTime.UtcNow;
        ClinicalExam = clinicalExam;
        ProgressNotes = progressNotes;
        TreatmentPlan = treatmentPlan;
    }

    public void Update(
        string? clinicalExam,
        string? progressNotes,
        string? treatmentPlan)
    {
        ClinicalExam = clinicalExam;
        ProgressNotes = progressNotes;
        TreatmentPlan = treatmentPlan;
    }
}
