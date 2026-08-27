namespace Application.DTOs.Odontogram.OdontogramTooth
{
    public class UpdateOdontogramToothRequestDto
    {
        public Guid ToothStatusId { get; set; }
        public string? Note { get; set; }
    }
}
