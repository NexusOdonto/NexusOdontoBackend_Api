namespace Application.DTOs.Auth;

public sealed class LoginRequestDto
{
    public string DocumentNumber { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
