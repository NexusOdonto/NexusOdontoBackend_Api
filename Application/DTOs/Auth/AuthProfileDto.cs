namespace Application.DTOs.Auth;

public sealed class AuthProfileDto
{
    public UserResponseDto User { get; set; } = new();

    public IReadOnlyList<string> Permissions { get; set; } = [];
}
