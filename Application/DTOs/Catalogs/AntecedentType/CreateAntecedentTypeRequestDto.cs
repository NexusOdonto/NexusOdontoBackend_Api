namespace Application.DTOs.Catalogs.AntecedentType
{
    public class CreateAntecedentTypeRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
    }
}
