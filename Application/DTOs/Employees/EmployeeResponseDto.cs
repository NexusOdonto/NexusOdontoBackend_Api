namespace Application.DTOs.Employees;

public sealed class EmployeeResponseDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public Guid JobTitleId { get; set; }
    public DateOnly HireDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
