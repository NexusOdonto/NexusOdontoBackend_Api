namespace Application.Contracts.Services;

public sealed class AuthSettings
{
    public string FrontendSuccessUrl { get; init; } = "https://nexusodonto.chatcampuslands.com/auth/callback";

    public string FrontendLinkUrl { get; init; } = "https://nexusodonto.chatcampuslands.com/auth/link-google";
}
