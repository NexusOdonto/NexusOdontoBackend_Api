using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Personas;

namespace Application.Contracts.Services.People
{
    public interface IPersonService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreatePersonaDto request);
        Task<object?> UpdateAsync(Guid id, UpdatePersonaDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
