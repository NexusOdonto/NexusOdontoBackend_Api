namespace Application.DTOs.Professionals;

public sealed class ProfessionalResponseDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string ProfessionalLicense { get; set; } = string.Empty;
    public IReadOnlyList<Guid> SpecialtyIds { get; set; } = [];
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
