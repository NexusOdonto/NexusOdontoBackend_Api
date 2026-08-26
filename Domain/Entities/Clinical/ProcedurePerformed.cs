using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.Clinical;

public class ProcedurePerformed : Entity
{
    public Guid ClinicalAttentionId { get; set; }
    public Guid? ServiceId { get; set; }
    public string Description { get; set; } = null!;
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
    public decimal? AmountCharged { get; set; }

    public ClinicalAttention? ClinicalAttention { get; set; }
    public Service? Service { get; set; }

    private ProcedurePerformed()
    {
    }

    public ProcedurePerformed(
        Guid clinicalAttentionId,
        string description,
        Guid? serviceId = null,
        DateTime? performedAt = null,
        decimal? amountCharged = null)
    {
        ClinicalAttentionId = clinicalAttentionId;
        Description = description;
        ServiceId = serviceId;
        PerformedAt = performedAt ?? DateTime.UtcNow;
        AmountCharged = amountCharged;
    }

    public void Update(
        string description,
        Guid? serviceId,
        DateTime performedAt,
        decimal? amountCharged)
    {
        Description = description;
        ServiceId = serviceId;
        PerformedAt = performedAt;
        AmountCharged = amountCharged;
    }
}
