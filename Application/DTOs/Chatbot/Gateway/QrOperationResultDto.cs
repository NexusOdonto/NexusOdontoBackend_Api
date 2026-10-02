namespace Application.DTOs.Chatbot.Gateway;

public sealed class QrOperationResultDto
{
    public string Status { get; set; } = "success";
    public bool? Unlinked { get; set; }
    public string? Message { get; set; }
}
