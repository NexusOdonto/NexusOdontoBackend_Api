using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.People;

public class Professional : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public string ProfessionalLicense { get; set; } = null!;

    public Employee? Employee { get; set; }
    public ICollection<Specialty> Specialties { get; set; } = [];

    private Professional()
    {
    }

    public Professional(Guid employeeId, string professionalLicense)
    {
        EmployeeId = employeeId;
        ProfessionalLicense = professionalLicense;
    }

    public void Update(string professionalLicense)
    {
        UpdatedAt = DateTime.UtcNow;
        ProfessionalLicense = professionalLicense;
    }
}
