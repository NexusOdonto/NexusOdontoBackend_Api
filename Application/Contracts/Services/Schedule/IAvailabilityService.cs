using Application.DTOs.Availabilities;

namespace Application.Contracts.Services.Schedule
{
    public interface IAvailabilityService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateAvailabilityDto request);
        Task<object?> UpdateAsync(Guid id, UpdateAvailabilityDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
