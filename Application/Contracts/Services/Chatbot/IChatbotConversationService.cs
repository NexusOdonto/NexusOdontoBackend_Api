using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Chatbot.ChatbotConversation;

namespace Application.Contracts.Services.Chatbot
{
    public interface IChatbotConversationService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateChatbotConversationRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
