namespace Application.DTOs.Chatbot.Gateway;

public sealed class ResumeConversationRequestDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public bool ClearHistory { get; set; }
}
