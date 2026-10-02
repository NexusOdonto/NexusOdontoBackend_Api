namespace Application.DTOs.Chatbot.Gateway;

public sealed class ResumeOperationResultDto
{
    public string Status { get; set; } = "success";
    public string? PhoneNumber { get; set; }
    public string? Message { get; set; }
}
