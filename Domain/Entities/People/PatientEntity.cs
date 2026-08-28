using Domain.Common;

namespace Domain.Entities.People;

public class PatientEntity : BaseEntity
{
    public Guid PersonId { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }

    public PersonEntity? PersonEntity { get; set; }

    private PatientEntity()
    {
    }

    public PatientEntity(Guid personId, string? emergencyContact, string? emergencyPhone)
    {
        PersonId = personId;
        EmergencyContact = emergencyContact;
        EmergencyPhone = emergencyPhone;
    }

    public void Update(string? emergencyContact, string? emergencyPhone)
    {
        UpdatedAt = DateTime.UtcNow;
        EmergencyContact = emergencyContact;
        EmergencyPhone = emergencyPhone;
    }
}
