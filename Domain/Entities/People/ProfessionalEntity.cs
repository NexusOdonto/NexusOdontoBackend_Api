using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.People;

public class ProfessionalEntity : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public string ProfessionalLicense { get; set; } = null!;

    public EmployeeEntity? EmployeeEntity { get; set; }
    public ICollection<SpecialtyEntity> Specialties { get; set; } = [];

    private ProfessionalEntity()
    {
    }

    public ProfessionalEntity(Guid employeeId, string professionalLicense)
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
