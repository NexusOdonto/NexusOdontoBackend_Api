namespace Application.DTOs.Catalogs.DentitionType
{
    public class CreateDentitionTypeRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
    }
}
