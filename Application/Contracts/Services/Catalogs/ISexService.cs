using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface ISexService
    {
        Task<IReadOnlyList<SexoDto>> GetAllAsync();
        Task<SexoDto?> GetByIdAsync(Guid id);
        Task<SexoDto> CreateAsync(SexoDto request);
        Task<SexoDto?> UpdateAsync(Guid id, SexoDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
