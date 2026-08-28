using Application.DTOs.Chatbot.ChatbotConversation;

namespace Application.Contracts.Services.Chatbot
{
    public interface IChatbotConversationService
    {
        Task<IReadOnlyList<ChatbotConversationResponseDto>> GetAllAsync();
        Task<ChatbotConversationResponseDto?> GetByIdAsync(Guid id);
        Task<ChatbotConversationResponseDto> CreateAsync(CreateChatbotConversationRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
