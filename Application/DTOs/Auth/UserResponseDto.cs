namespace Application.DTOs.Auth;

public sealed class UserResponseDto
{
    public Guid Id { get; set; }

    public Guid PersonId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string DocumentNumber { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string Email { get; set; } = string.Empty;

    public IReadOnlyList<Guid> RoleIds { get; set; } = [];
}
