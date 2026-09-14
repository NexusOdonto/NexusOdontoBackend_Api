namespace Application.Contracts.Services;

public sealed class ChatbotSettings
{
    public string BaseUrl { get; init; } = string.Empty;

    public string InternalSecret { get; init; } = string.Empty;

    public string MessagePath { get; init; } = "/api/v1/agent/send-message";

    public int TimeoutSeconds { get; init; } = 30;
}
