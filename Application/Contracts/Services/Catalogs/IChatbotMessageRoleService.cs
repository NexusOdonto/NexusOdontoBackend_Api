using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.ChatbotMessageRole;

namespace Application.Contracts.Services.Catalogs
{
    public interface IChatbotMessageRoleService
    {
        Task<IReadOnlyList<ChatbotMessageRoleDto>> GetAllAsync();
        Task<ChatbotMessageRoleDto?> GetByIdAsync(Guid id);
        Task<ChatbotMessageRoleDto> CreateAsync(CreateChatbotMessageRoleRequestDto request);
        Task<ChatbotMessageRoleDto?> UpdateAsync(Guid id, UpdateChatbotMessageRoleRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
