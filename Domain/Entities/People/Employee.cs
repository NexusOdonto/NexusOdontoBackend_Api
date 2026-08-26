using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.People;

public class Employee : BaseEntity
{
    public Guid PersonId { get; set; }
    public Guid JobTitleId { get; set; }
    public DateTime HireDate { get; set; }

    public Person? Person { get; set; }
    public JobTitle? JobTitle { get; set; }
    public Professional? Professional { get; set; }

    private Employee()
    {
    }

    public Employee(Guid personId, Guid jobTitleId, DateTime hireDate)
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
