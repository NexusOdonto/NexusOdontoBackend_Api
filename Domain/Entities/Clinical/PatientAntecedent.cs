using Domain.Common;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Domain.Entities.Clinical;

public class PatientAntecedent : Entity
{
    public Guid PatientId { get; set; }
    public Guid AntecedentTypeId { get; set; }
    public string Description { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public Guid RegisteredByUserId { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public Patient? Patient { get; set; }
    public AntecedentType? AntecedentType { get; set; }
    public User? RegisteredByUser { get; set; }

    private PatientAntecedent()
    {
    }

    public PatientAntecedent(
        Guid patientId,
        Guid antecedentTypeId,
        string description,
        Guid registeredByUserId)
    {
        PatientId = patientId;
        AntecedentTypeId = antecedentTypeId;
        Description = description;
        RegisteredByUserId = registeredByUserId;
    }

    public void Update(string description, bool isActive)
    {
        Description = description;
        IsActive = isActive;
    }
}
