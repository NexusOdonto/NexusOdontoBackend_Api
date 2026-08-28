using Application.DTOs.Audit;

namespace Application.Contracts.Services.Audit
{
    public interface IAuditEventService
    {
        Task<IReadOnlyList<AuditEventResponseDto>> GetAllAsync();
        Task<AuditEventResponseDto?> GetByIdAsync(Guid id);
        Task<AuditEventResponseDto> CreateAsync(CreateAuditEventRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
