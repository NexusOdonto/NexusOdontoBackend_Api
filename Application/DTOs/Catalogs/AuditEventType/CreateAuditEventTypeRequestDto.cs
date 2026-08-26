namespace Application.DTOs.Catalogs.AuditEventType
{
    public class CreateAuditEventTypeRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public int? RiskLevel { get; set; }
        public int? SortOrder { get; set; }
    }
}
