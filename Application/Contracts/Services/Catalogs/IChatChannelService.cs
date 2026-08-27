using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.ChatChannel;

namespace Application.Contracts.Services.Catalogs
{
    public interface IChatChannelService
    {
        Task<IReadOnlyList<ChatChannelDto>> GetAllAsync();
        Task<ChatChannelDto?> GetByIdAsync(Guid id);
        Task<ChatChannelDto> CreateAsync(CreateChatChannelRequestDto request);
        Task<ChatChannelDto?> UpdateAsync(Guid id, UpdateChatChannelRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
