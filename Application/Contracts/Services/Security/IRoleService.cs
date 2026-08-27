using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Roles;

namespace Application.Contracts.Services.Security
{
    public interface IRoleService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateRolDto request);
        Task<object?> UpdateAsync(Guid id, UpdateRolDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
