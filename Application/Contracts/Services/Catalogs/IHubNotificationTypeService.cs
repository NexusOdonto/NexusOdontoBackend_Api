using Application.DTOs.Catalogs.HubNotificationType;

namespace Application.Contracts.Services.Catalogs
{
    public interface IHubNotificationTypeService
    {
        Task<IReadOnlyList<HubNotificationTypeDto>> GetAllAsync();
        Task<HubNotificationTypeDto?> GetByIdAsync(Guid id);
        Task<HubNotificationTypeDto> CreateAsync(HubNotificationTypeDto request);
        Task<HubNotificationTypeDto?> UpdateAsync(Guid id, HubNotificationTypeDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
