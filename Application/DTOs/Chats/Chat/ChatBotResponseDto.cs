using System.Collections.Generic;

namespace Application.DTOs.Chats.Chat
{
    public class ChatBotResponseDto
    {
        public string Response { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string? InvoiceNumber { get; set; }
        public string? SaleOrigin { get; set; }
        public List<ChatBotProductDto> Products { get; set; } = new();
    }
}
