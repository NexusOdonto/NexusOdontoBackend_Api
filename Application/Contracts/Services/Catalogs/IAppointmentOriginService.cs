using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface IAppointmentOriginService
    {
        Task<IReadOnlyList<OrigenCitaDto>> GetAllAsync();
        Task<OrigenCitaDto?> GetByIdAsync(Guid id);
        Task<OrigenCitaDto> CreateAsync(OrigenCitaDto request);
        Task<OrigenCitaDto?> UpdateAsync(Guid id, OrigenCitaDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
