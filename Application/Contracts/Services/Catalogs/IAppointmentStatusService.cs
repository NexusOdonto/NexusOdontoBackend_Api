using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface IAppointmentStatusService
    {
        Task<IReadOnlyList<EstadoCitaDto>> GetAllAsync();
        Task<EstadoCitaDto?> GetByIdAsync(Guid id);
        Task<EstadoCitaDto> CreateAsync(EstadoCitaDto request);
        Task<EstadoCitaDto?> UpdateAsync(Guid id, EstadoCitaDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
