using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.SupportTicketReason;

namespace Application.Contracts.Services.Catalogs
{
    public interface ISupportTicketReasonService
    {
        Task<IReadOnlyList<SupportTicketReasonDto>> GetAllAsync();
        Task<SupportTicketReasonDto?> GetByIdAsync(Guid id);
        Task<SupportTicketReasonDto> CreateAsync(CreateSupportTicketReasonRequestDto request);
        Task<SupportTicketReasonDto?> UpdateAsync(Guid id, UpdateSupportTicketReasonRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
