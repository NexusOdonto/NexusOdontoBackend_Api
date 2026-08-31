namespace Application.DTOs.Professionals;

public class UpdateProfessionalDto
{
    public string ProfessionalLicense { get; set; } = string.Empty;
    public List<Guid>? SpecialtyIds { get; set; }
    public bool IsActive { get; set; }
}
