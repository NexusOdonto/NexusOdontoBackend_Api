using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.People;

public class EmployeeEntity : BaseEntity
{
    public Guid PersonId { get; set; }
    public Guid JobTitleId { get; set; }
    public string EmployeeCode { get; set; } = null!;
    public DateOnly HireDate { get; set; }

    public PersonEntity? PersonEntity { get; set; }
    public JobTitleEntity? JobTitleEntity { get; set; }
    public ProfessionalEntity? ProfessionalEntity { get; set; }

    private EmployeeEntity()
    {
    }

    public EmployeeEntity(Guid personId, Guid jobTitleId, DateOnly hireDate, string employeeCode)
    {
        PersonId = personId;
        JobTitleId = jobTitleId;
        HireDate = hireDate;
        EmployeeCode = employeeCode;
    }

    public void Update(Guid jobTitleId, DateOnly hireDate, string employeeCode)
    {
        UpdatedAt = DateTime.UtcNow;
        JobTitleId = jobTitleId;
        HireDate = hireDate;
        EmployeeCode = employeeCode;
    }
}
