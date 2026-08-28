namespace Application.DTOs.Permissions;

public sealed class PermissionResponseDto
{
    public Guid Id { get; set; }
    public string Module { get; set; } = string.Empty;
    public Guid ActionPermissionId { get; set; }
    public string? Description { get; set; }
}
