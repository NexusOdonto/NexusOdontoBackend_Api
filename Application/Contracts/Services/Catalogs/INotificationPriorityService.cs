using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface INotificationPriorityService
    {
        Task<IReadOnlyList<PrioridadNotificacionDto>> GetAllAsync();
        Task<PrioridadNotificacionDto?> GetByIdAsync(Guid id);
        Task<PrioridadNotificacionDto> CreateAsync(PrioridadNotificacionDto request);
        Task<PrioridadNotificacionDto?> UpdateAsync(Guid id, PrioridadNotificacionDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
