using Application.DTOs.Catalogs.SessionStatus;

namespace Application.Contracts.Services.Catalogs
{
    public interface ISessionStatusService
    {
        Task<IReadOnlyList<SessionStatusDto>> GetAllAsync();
        Task<SessionStatusDto?> GetByIdAsync(Guid id);
        Task<SessionStatusDto> CreateAsync(SessionStatusDto request);
        Task<SessionStatusDto?> UpdateAsync(Guid id, SessionStatusDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
