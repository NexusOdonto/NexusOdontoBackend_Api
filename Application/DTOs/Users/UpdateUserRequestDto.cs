namespace Application.DTOs.Users;

public sealed class UpdateUserRequestDto
{
    public Guid RoleId { get; set; }

    public bool IsActive { get; set; } = true;
}
