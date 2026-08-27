using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.UsuarioRoles;

namespace Application.Contracts.Services.Security
{
    public interface IUserRoleService
    {
        Task<IReadOnlyList<UsuarioRolDto>> GetAllAsync();
        Task<UsuarioRolDto?> GetByIdAsync(Guid id);
        Task<UsuarioRolDto> CreateAsync(UsuarioRolDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
