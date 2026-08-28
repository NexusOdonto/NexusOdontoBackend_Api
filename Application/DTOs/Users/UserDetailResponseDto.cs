namespace Application.DTOs.Users;

public sealed class UserDetailResponseDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public DateTime? LastAccessAt { get; set; }
    public IReadOnlyList<Guid> RoleIds { get; set; } = [];
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
