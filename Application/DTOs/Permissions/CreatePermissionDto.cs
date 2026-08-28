namespace Application.DTOs.Permissions
{
    public class CreatePermissionDto
    {
        public string Module { get; set; } = string.Empty;
        public Guid ActionPermissionId { get; set; }
        public string? Description { get; set; }
    }
}
