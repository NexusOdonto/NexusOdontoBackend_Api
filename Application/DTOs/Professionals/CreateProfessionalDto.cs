namespace Application.DTOs.Professionals;

public class CreateProfessionalDto
{
    public Guid EmployeeId { get; set; }
    public Guid? PersonId { get; set; }
    public string ProfessionalLicense { get; set; } = string.Empty;
    public string? LicenseNumber { get; set; }
    public List<Guid> SpecialtyIds { get; set; } = [];
    public Guid? SpecialtyId { get; set; }
}
