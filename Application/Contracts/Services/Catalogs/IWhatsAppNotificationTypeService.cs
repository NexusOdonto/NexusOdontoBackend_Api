using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface IWhatsAppNotificationTypeService
    {
        Task<IReadOnlyList<TipoNotificacionWhatsappDto>> GetAllAsync();
        Task<TipoNotificacionWhatsappDto?> GetByIdAsync(Guid id);
        Task<TipoNotificacionWhatsappDto> CreateAsync(TipoNotificacionWhatsappDto request);
        Task<TipoNotificacionWhatsappDto?> UpdateAsync(Guid id, TipoNotificacionWhatsappDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
