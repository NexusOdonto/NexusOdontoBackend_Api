namespace Application.DTOs.Patients
{
    public class CreatePatientDto
    {
        public Guid PersonId { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
    }
}
