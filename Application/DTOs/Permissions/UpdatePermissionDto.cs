namespace Application.DTOs.Permissions
{
    public class UpdatePermissionDto
    {
        public string Module { get; set; } = string.Empty;
        public Guid ActionPermissionId { get; set; }
        public string? Description { get; set; }
    }
}
