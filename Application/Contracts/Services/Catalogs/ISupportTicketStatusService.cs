using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.SupportTicketStatus;

namespace Application.Contracts.Services.Catalogs
{
    public interface ISupportTicketStatusService
    {
        Task<IReadOnlyList<SupportTicketStatusDto>> GetAllAsync();
        Task<SupportTicketStatusDto?> GetByIdAsync(Guid id);
        Task<SupportTicketStatusDto> CreateAsync(CreateSupportTicketStatusRequestDto request);
        Task<SupportTicketStatusDto?> UpdateAsync(Guid id, UpdateSupportTicketStatusRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
