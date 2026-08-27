namespace Application.DTOs.Auth;

public sealed class RegisterRequestDto
{
    public Guid PersonId { get; set; }

    public string Password { get; set; } = string.Empty;

    public Guid RoleId { get; set; }
}
