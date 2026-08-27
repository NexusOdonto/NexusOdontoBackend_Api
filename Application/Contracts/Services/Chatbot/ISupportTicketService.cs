using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Support.SupportTicket;

namespace Application.Contracts.Services.Chatbot
{
    public interface ISupportTicketService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateSupportTicketRequestDto request);
        Task<object?> UpdateAsync(Guid id, UpdateSupportTicketRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
