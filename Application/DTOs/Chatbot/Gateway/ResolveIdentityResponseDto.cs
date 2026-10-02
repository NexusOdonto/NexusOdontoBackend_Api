namespace Application.DTOs.Chatbot.Gateway;

public sealed class ResolveIdentityResponseDto
{
    public string Status { get; set; } = "unresolved";
    public string? Phone { get; set; }
    public string? Identifier { get; set; }
    public string? Via { get; set; }
}
