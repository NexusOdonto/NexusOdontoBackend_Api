namespace Application.Contracts.Services;

public sealed class GoogleSettings
{
    public string ClientId { get; init; } = string.Empty;

    public string ClientSecret { get; init; } = string.Empty;
}
