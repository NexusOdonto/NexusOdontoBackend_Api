namespace Application.DTOs.Employees
{
    public class UpdateEmployeeDto
    {
        public Guid JobTitleId { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
    }
}
