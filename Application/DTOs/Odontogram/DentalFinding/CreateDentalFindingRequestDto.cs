namespace Application.DTOs.Odontogram.DentalFinding
{
    public class CreateDentalFindingRequestDto
    {
        public Guid OdontogramToothId { get; set; }
        public Guid? DentalSurfaceId { get; set; }
        public Guid FindingTypeId { get; set; }
        public Guid FindingStatusId { get; set; }
        public string? Note { get; set; }
    }
}
