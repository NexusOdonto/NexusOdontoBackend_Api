using Domain.Common;

namespace Domain.Entities.People;

public class Patient : BaseEntity
{
    public Guid PersonId { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }

    public Person? Person { get; set; }

    private Patient()
    {
    }

    public Patient(Guid personId, string? emergencyContact, string? emergencyPhone)
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
