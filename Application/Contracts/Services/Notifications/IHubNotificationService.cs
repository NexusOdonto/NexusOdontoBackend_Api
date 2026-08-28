using Application.DTOs.Notifications.HubNotification;

namespace Application.Contracts.Services.Notifications
{
    public interface IHubNotificationService
    {
        Task<IReadOnlyList<HubNotificationResponseDto>> GetAllAsync();
        Task<HubNotificationResponseDto?> GetByIdAsync(Guid id);
        Task<HubNotificationResponseDto> CreateAsync(CreateHubNotificationRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
