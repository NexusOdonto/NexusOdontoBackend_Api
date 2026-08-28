namespace Application.DTOs.Employees
{
    public class CreateEmployeeDto
    {
        public Guid PersonId { get; set; }
        public Guid JobTitleId { get; set; }
        public DateTime HireDate { get; set; }
    }
}
