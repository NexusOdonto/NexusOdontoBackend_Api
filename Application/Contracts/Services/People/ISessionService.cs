using Application.DTOs.Sessions;

namespace Application.Contracts.Services.People
{
    public interface ISessionService
    {
        Task<IReadOnlyList<SessionDto>> GetAllAsync();
        Task<SessionDto?> GetByIdAsync(Guid id);
        Task<SessionDto> CreateAsync(SessionDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
