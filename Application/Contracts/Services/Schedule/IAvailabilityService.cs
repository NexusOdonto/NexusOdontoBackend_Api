using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Disponibilidades;

namespace Application.Contracts.Services.Schedule
{
    public interface IAvailabilityService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateDisponibilidadDto request);
        Task<object?> UpdateAsync(Guid id, UpdateDisponibilidadDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
