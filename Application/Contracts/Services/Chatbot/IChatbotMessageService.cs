using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Chatbot.ChatbotMessage;

namespace Application.Contracts.Services.Chatbot
{
    public interface IChatbotMessageService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateChatbotMessageRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
