namespace Application.DTOs.Catalogs.FindingType
{
    public class UpdateFindingTypeRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ColorHex { get; set; }
        public string? VisualSymbol { get; set; }
        public bool RequiresSurface { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
    }
}
