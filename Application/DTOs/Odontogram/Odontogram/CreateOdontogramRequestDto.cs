namespace Application.DTOs.Odontogram.Odontogram
{
    public class CreateOdontogramRequestDto
    {
        public Guid ClinicalHistoryId { get; set; }
        public Guid? ClinicalAttentionId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string? Notes { get; set; }
    }
}
