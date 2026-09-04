using Application.DTOs.Chatbot.ChatbotConversation;
using Application.DTOs.Chatbot.ChatbotMessage;

namespace Application.Contracts.Services.Chatbot
{
    public interface IChatbotConversationService
    {
        Task<IReadOnlyList<ChatbotConversationResponseDto>> GetAllAsync();
        Task<ChatbotConversationResponseDto?> GetByIdAsync(Guid id);
        Task<ChatbotConversationResponseDto> CreateAsync(CreateChatbotConversationRequestDto request);
        Task<ChatbotConversationResponseDto?> UpdateAsync(Guid id, UpdateChatbotConversationRequestDto request);
        Task<IReadOnlyList<ChatbotMessageResponseDto>> GetMessagesAsync(Guid conversationId);
        Task<bool> DeleteAsync(Guid id);
    }
}
