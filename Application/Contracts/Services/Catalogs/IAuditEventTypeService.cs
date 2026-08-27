using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.AuditEventType;

namespace Application.Contracts.Services.Catalogs
{
    public interface IAuditEventTypeService
    {
        Task<IReadOnlyList<AuditEventTypeDto>> GetAllAsync();
        Task<AuditEventTypeDto?> GetByIdAsync(Guid id);
        Task<AuditEventTypeDto> CreateAsync(CreateAuditEventTypeRequestDto request);
        Task<AuditEventTypeDto?> UpdateAsync(Guid id, UpdateAuditEventTypeRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
