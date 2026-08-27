using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface IActionPermissionService
    {
        Task<IReadOnlyList<AccionPermisoDto>> GetAllAsync();
        Task<AccionPermisoDto?> GetByIdAsync(Guid id);
        Task<AccionPermisoDto> CreateAsync(AccionPermisoDto request);
        Task<AccionPermisoDto?> UpdateAsync(Guid id, AccionPermisoDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
