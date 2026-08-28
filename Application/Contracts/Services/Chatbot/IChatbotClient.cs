using System.Threading.Tasks;
using Application.DTOs.Chats.Chat;

namespace Application.Contracts.Services.Chatbot
{
    public interface IChatbotClient
    {
        Task<ChatBotResponseDto> SendMessageAsync(string sessionId, string message);
    }
}
