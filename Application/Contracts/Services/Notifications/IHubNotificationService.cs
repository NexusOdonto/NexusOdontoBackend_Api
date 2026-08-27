using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Notifications.HubNotification;

namespace Application.Contracts.Services.Notifications
{
    public interface IHubNotificationService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateHubNotificationRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
