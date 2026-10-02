namespace Application.DTOs.Chatbot.Gateway;

public sealed class QrDataResponseDto
{
    public bool Connected { get; set; }
    public string State { get; set; } = "close";
    public string? Base64 { get; set; }
    public string? InstanceName { get; set; }
}
