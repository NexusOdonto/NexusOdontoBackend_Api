namespace Application.DTOs.Support.SupportTicket;

public record ReplyTicketRequestDto(string Content, string? MessageRoleId = null);
