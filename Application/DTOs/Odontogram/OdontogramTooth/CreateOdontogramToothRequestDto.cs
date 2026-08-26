namespace Application.DTOs.Odontogram.OdontogramTooth
{
    public class CreateOdontogramToothRequestDto
    {
        public Guid OdontogramId { get; set; }
        public string ToothCode { get; set; } = string.Empty;
        public Guid ToothStatusId { get; set; }
        public string? Note { get; set; }
    }
}
