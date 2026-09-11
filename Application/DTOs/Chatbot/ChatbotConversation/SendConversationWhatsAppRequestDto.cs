namespace Application.DTOs.Chatbot.ChatbotConversation;

public record SendConversationWhatsAppRequestDto(string Content, string? MessageRoleId = null);
