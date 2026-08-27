namespace Application.DTOs.Catalogs.ToothStatus
{
    public class ToothStatusDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ColorHex { get; set; }
        public string? VisualCode { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
    }
}
