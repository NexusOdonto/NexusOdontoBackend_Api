using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.ChatbotConversationStatus;

namespace Application.Contracts.Services.Catalogs
{
    public interface IChatbotConversationStatusService
    {
        Task<IReadOnlyList<ChatbotConversationStatusDto>> GetAllAsync();
        Task<ChatbotConversationStatusDto?> GetByIdAsync(Guid id);
        Task<ChatbotConversationStatusDto> CreateAsync(CreateChatbotConversationStatusRequestDto request);
        Task<ChatbotConversationStatusDto?> UpdateAsync(Guid id, UpdateChatbotConversationStatusRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
