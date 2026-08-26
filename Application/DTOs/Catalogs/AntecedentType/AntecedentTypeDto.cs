namespace Application.DTOs.Catalogs.AntecedentType
{
    public class AntecedentTypeDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
    }
}
