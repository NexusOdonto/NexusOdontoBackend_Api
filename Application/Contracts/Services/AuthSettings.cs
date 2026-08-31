namespace Application.Contracts.Services;

public sealed class AuthSettings
{
    public string FrontendSuccessUrl { get; init; } = "http://localhost:3000/auth/callback";

    public string FrontendLinkUrl { get; init; } = "http://localhost:3000/auth/link-google";
}
