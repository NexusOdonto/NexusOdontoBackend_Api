using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Citas;

namespace Application.Contracts.Services.Schedule
{
    public interface IAppointmentService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateCitaDto request);
        Task<object?> UpdateAsync(Guid id, UpdateCitaDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
