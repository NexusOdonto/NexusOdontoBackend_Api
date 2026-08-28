using Application.DTOs.Catalogs.HubNotificationStatus;

namespace Application.Contracts.Services.Catalogs
{
    public interface IHubNotificationStatusService
    {
        Task<IReadOnlyList<HubNotificationStatusDto>> GetAllAsync();
        Task<HubNotificationStatusDto?> GetByIdAsync(Guid id);
        Task<HubNotificationStatusDto> CreateAsync(HubNotificationStatusDto request);
        Task<HubNotificationStatusDto?> UpdateAsync(Guid id, HubNotificationStatusDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
