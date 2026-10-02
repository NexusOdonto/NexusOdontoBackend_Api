namespace Application.DTOs.Chatbot.Gateway;

public sealed class ResolveIdentityRequestDto
{
    public string Identifier { get; set; } = string.Empty;
    public Guid? ConversationId { get; set; }
}
