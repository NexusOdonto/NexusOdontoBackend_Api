namespace Application.DTOs.Clinical.ClinicalHistory
{
    public class UpdateClinicalHistoryRequestDto
    {
        public bool IsActive { get; set; }
        public string? GeneralNotes { get; set; }
    }
}
