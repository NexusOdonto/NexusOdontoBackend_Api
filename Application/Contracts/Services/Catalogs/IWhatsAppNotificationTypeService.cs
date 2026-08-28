using Application.DTOs.Catalogs.WhatsAppNotificationType;

namespace Application.Contracts.Services.Catalogs
{
    public interface IWhatsAppNotificationTypeService
    {
        Task<IReadOnlyList<WhatsAppNotificationTypeDto>> GetAllAsync();
        Task<WhatsAppNotificationTypeDto?> GetByIdAsync(Guid id);
        Task<WhatsAppNotificationTypeDto> CreateAsync(WhatsAppNotificationTypeDto request);
        Task<WhatsAppNotificationTypeDto?> UpdateAsync(Guid id, WhatsAppNotificationTypeDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
