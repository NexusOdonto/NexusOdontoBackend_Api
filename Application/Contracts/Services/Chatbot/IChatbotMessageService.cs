using Application.DTOs.Chatbot.ChatbotMessage;

namespace Application.Contracts.Services.Chatbot
{
    public interface IChatbotMessageService
    {
        Task<IReadOnlyList<ChatbotMessageResponseDto>> GetAllAsync();
        Task<ChatbotMessageResponseDto?> GetByIdAsync(Guid id);
        Task<ChatbotMessageResponseDto> CreateAsync(CreateChatbotMessageRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
