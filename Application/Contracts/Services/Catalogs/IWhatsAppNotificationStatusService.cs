using Application.DTOs.Catalogs.WhatsAppNotificationStatus;

namespace Application.Contracts.Services.Catalogs
{
    public interface IWhatsAppNotificationStatusService
    {
        Task<IReadOnlyList<WhatsAppNotificationStatusDto>> GetAllAsync();
        Task<WhatsAppNotificationStatusDto?> GetByIdAsync(Guid id);
        Task<WhatsAppNotificationStatusDto> CreateAsync(WhatsAppNotificationStatusDto request);
        Task<WhatsAppNotificationStatusDto?> UpdateAsync(Guid id, WhatsAppNotificationStatusDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
