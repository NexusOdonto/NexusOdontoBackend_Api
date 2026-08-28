using Domain.Common;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Domain.Entities.Clinical;

public class PatientAntecedentEntity : Entity
{
    public Guid PatientId { get; set; }
    public Guid AntecedentTypeId { get; set; }
    public string Description { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public Guid RegisteredByUserId { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public PatientEntity? PatientEntity { get; set; }
    public AntecedentTypeEntity? AntecedentTypeEntity { get; set; }
    public UserEntity? RegisteredByUser { get; set; }

    private PatientAntecedentEntity()
    {
    }

    public PatientAntecedentEntity(
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
