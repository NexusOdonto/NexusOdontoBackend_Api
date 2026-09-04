namespace Application.DTOs.Clinical.PatientAntecedent
{
    public class CreatePatientAntecedentRequestDto
    {
        public Guid PatientId { get; set; }
        public Guid AntecedentTypeId { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid? RegisteredByUserId { get; set; }
    }
}
