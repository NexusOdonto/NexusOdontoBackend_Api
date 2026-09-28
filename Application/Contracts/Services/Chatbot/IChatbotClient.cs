using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Chats.Chat;
using Application.DTOs.Chats.ScheduleChange;

namespace Application.Contracts.Services.Chatbot
{
    public interface IChatbotClient
    {
        Task<ChatBotResponseDto> SendMessageAsync(string sessionId, string message);
        Task<bool> SendAgentMessageAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
        Task<bool> ResumeConversationAsync(string phoneNumber, bool clearHistory = false, CancellationToken cancellationToken = default);
        Task<ScheduleChangeNotifyResult> NotifyScheduleChangeAsync(ScheduleChangeNotificationDto notification, CancellationToken cancellationToken = default);
    }
}
