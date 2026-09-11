namespace Application.DTOs.Support.SupportTicket;

/// <summary>
/// Resolve a support ticket. When <see cref="ReturnToBot"/> is true (default),
/// the linked conversation is set back to ACTIVA and the chatbot resumes.
/// When false, the conversation is closed (CERRADA) and the bot is not resumed.
/// </summary>
public class ResolveTicketRequestDto
{
    public string? Resolution { get; set; }
    public bool ReturnToBot { get; set; } = true;
}
