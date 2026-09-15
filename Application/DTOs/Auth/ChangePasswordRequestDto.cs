namespace Application.DTOs.Auth;

public sealed class ChangePasswordRequestDto
{
    public string NewPassword { get; set; } = string.Empty;
}
