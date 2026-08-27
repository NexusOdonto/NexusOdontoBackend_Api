using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface ISpecialtyService
    {
        Task<IReadOnlyList<EspecialidadDto>> GetAllAsync();
        Task<EspecialidadDto?> GetByIdAsync(Guid id);
        Task<EspecialidadDto> CreateAsync(EspecialidadDto request);
        Task<EspecialidadDto?> UpdateAsync(Guid id, EspecialidadDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
