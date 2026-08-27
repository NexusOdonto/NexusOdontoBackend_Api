using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Usuarios;

namespace Application.Contracts.Services.People
{
    public interface IUserService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateUsuarioDto request);
        Task<object?> UpdateAsync(Guid id, UpdateUsuarioDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
