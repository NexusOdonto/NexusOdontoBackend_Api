using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.People;

public class EmployeeEntity : BaseEntity
{
    public Guid PersonId { get; set; }
    public Guid JobTitleId { get; set; }
    public DateTime HireDate { get; set; }

    public PersonEntity? PersonEntity { get; set; }
    public JobTitleEntity? JobTitleEntity { get; set; }
    public ProfessionalEntity? ProfessionalEntity { get; set; }

    private EmployeeEntity()
    {
    }

    public EmployeeEntity(Guid personId, Guid jobTitleId, DateTime hireDate)
    {
        PersonId = personId;
        JobTitleId = jobTitleId;
        HireDate = hireDate;
    }

    public void Update(Guid jobTitleId, DateTime hireDate)
    {
        UpdatedAt = DateTime.UtcNow;
        JobTitleId = jobTitleId;
        HireDate = hireDate;
    }
}
