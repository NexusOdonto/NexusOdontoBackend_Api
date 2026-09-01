namespace Application.DTOs.Auth;

public sealed class GoogleLinkRequestDto
{
    public string LinkToken { get; set; } = string.Empty;

    public string LoginId { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
