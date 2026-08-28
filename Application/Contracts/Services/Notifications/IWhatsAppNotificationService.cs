using Application.DTOs.Notifications.WhatsAppNotification;

namespace Application.Contracts.Services.Notifications
{
    public interface IWhatsAppNotificationService
    {
        Task<IReadOnlyList<WhatsAppNotificationResponseDto>> GetAllAsync();
        Task<WhatsAppNotificationResponseDto?> GetByIdAsync(Guid id);
        Task<WhatsAppNotificationResponseDto> CreateAsync(CreateWhatsAppNotificationRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
