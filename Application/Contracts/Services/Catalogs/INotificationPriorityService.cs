using Application.DTOs.Catalogs.NotificationPriority;

namespace Application.Contracts.Services.Catalogs
{
    public interface INotificationPriorityService
    {
        Task<IReadOnlyList<NotificationPriorityDto>> GetAllAsync();
        Task<NotificationPriorityDto?> GetByIdAsync(Guid id);
        Task<NotificationPriorityDto> CreateAsync(NotificationPriorityDto request);
        Task<NotificationPriorityDto?> UpdateAsync(Guid id, NotificationPriorityDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
