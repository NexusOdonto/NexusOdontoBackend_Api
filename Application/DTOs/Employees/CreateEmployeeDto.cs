namespace Application.DTOs.Employees;

public class CreateEmployeeDto
{
    public Guid PersonId { get; set; }
    public Guid JobTitleId { get; set; }
    public DateOnly HireDate { get; set; }
    public string? EmployeeCode { get; set; }
}
