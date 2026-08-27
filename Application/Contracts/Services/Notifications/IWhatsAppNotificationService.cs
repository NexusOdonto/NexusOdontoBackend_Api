using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Notifications.WhatsAppNotification;

namespace Application.Contracts.Services.Notifications
{
    public interface IWhatsAppNotificationService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateWhatsAppNotificationRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
