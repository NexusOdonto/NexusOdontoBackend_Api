using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Permisos;

namespace Application.Contracts.Services.Security
{
    public interface IPermissionService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreatePermisoDto request);
        Task<object?> UpdateAsync(Guid id, UpdatePermisoDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
