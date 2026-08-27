using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface ISessionStatusService
    {
        Task<IReadOnlyList<EstadoSesionDto>> GetAllAsync();
        Task<EstadoSesionDto?> GetByIdAsync(Guid id);
        Task<EstadoSesionDto> CreateAsync(EstadoSesionDto request);
        Task<EstadoSesionDto?> UpdateAsync(Guid id, EstadoSesionDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
