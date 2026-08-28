namespace Application.DTOs.Professionals
{
    public class UpdateProfessionalDto
    {
        public string ProfessionalLicense { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
