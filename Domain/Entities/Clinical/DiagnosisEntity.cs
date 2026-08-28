using Domain.Common;

namespace Domain.Entities.Clinical;

public class DiagnosisEntity : Entity
{
    public Guid ClinicalAttentionId { get; set; }
    public string? Code { get; set; }
    public string Description { get; set; } = null!;
    public string Type { get; set; } = "DENTAL";

    public ClinicalAttentionEntity? ClinicalAttentionEntity { get; set; }

    private DiagnosisEntity()
    {
    }

    public DiagnosisEntity(
        Guid clinicalAttentionId,
        string description,
        string? code = null,
        string type = "DENTAL")
    {
        ClinicalAttentionId = clinicalAttentionId;
        Description = description;
        Code = code;
        Type = type;
    }

    public void Update(string description, string? code, string type)
    {
        Description = description;
        Code = code;
        Type = type;
    }
}
