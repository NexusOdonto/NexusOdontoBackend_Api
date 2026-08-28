namespace Application.DTOs.Patients
{
    public class UpdatePatientDto
    {
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
        public bool IsActive { get; set; }
    }
}
