namespace Application.DTOs.Catalogs.FindingStatus
{
    public class CreateFindingStatusRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ColorHex { get; set; }
        public int? SortOrder { get; set; }
    }
}
