using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.People;

public class Person : BaseEntity
{
    public Guid DocumentTypeId { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public Guid? SexId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    public DocumentType? DocumentType { get; set; }
    public Sex? Sex { get; set; }
    public Patient? Patient { get; set; }
    public Employee? Employee { get; set; }
    public User? User { get; set; }

    private Person()
    {
    }

    public Person(
        Guid documentTypeId,
        string documentNumber,
        string firstName,
        string lastName,
        DateTime? dateOfBirth = null,
        Guid? sexId = null,
        string? phone = null,
        string? email = null,
        string? address = null)
    {
        DocumentTypeId = documentTypeId;
        DocumentNumber = documentNumber;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        SexId = sexId;
        Phone = phone;
        Email = email;
        Address = address;
    }

    public void Update(
        string firstName,
        string lastName,
        DateTime? dateOfBirth,
        Guid? sexId,
        string? phone,
        string? email,
        string? address)
    {
        UpdatedAt = DateTime.UtcNow;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        SexId = sexId;
        Phone = phone;
        Email = email;
        Address = address;
    }
}
