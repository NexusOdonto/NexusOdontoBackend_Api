namespace Application.DTOs.Clinical.PatientAntecedent
{
    public class UpdatePatientAntecedentRequestDto
    {
        public Guid AntecedentTypeId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
