using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface IWhatsAppNotificationStatusService
    {
        Task<IReadOnlyList<EstadoNotificacionWhatsappDto>> GetAllAsync();
        Task<EstadoNotificacionWhatsappDto?> GetByIdAsync(Guid id);
        Task<EstadoNotificacionWhatsappDto> CreateAsync(EstadoNotificacionWhatsappDto request);
        Task<EstadoNotificacionWhatsappDto?> UpdateAsync(Guid id, EstadoNotificacionWhatsappDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
