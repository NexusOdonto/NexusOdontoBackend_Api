using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Profesionales;

namespace Application.Contracts.Services.People
{
    public interface IProfessionalService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateProfesionalDto request);
        Task<object?> UpdateAsync(Guid id, UpdateProfesionalDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
