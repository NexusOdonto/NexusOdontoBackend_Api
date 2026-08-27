using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface IHubNotificationStatusService
    {
        Task<IReadOnlyList<EstadoNotificacionHubDto>> GetAllAsync();
        Task<EstadoNotificacionHubDto?> GetByIdAsync(Guid id);
        Task<EstadoNotificacionHubDto> CreateAsync(EstadoNotificacionHubDto request);
        Task<EstadoNotificacionHubDto?> UpdateAsync(Guid id, EstadoNotificacionHubDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
