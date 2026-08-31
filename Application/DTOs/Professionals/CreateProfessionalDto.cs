namespace Application.DTOs.Professionals;

public class CreateProfessionalDto
{
    public Guid EmployeeId { get; set; }
    public string ProfessionalLicense { get; set; } = string.Empty;
    public List<Guid> SpecialtyIds { get; set; } = [];
}
