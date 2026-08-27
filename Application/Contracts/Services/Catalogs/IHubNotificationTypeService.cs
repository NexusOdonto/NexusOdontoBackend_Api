using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface IHubNotificationTypeService
    {
        Task<IReadOnlyList<TipoNotificacionHubDto>> GetAllAsync();
        Task<TipoNotificacionHubDto?> GetByIdAsync(Guid id);
        Task<TipoNotificacionHubDto> CreateAsync(TipoNotificacionHubDto request);
        Task<TipoNotificacionHubDto?> UpdateAsync(Guid id, TipoNotificacionHubDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
