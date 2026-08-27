namespace Application.DTOs.Clinical.ProcedurePerformed
{
    public class CreateProcedurePerformedRequestDto
    {
        public Guid ClinicalAttentionId { get; set; }
        public Guid? ServiceId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal? AmountCharged { get; set; }
    }
}
