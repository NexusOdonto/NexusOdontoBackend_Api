using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Pacientes;

namespace Application.Contracts.Services.People
{
    public interface IPatientService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreatePacienteDto request);
        Task<object?> UpdateAsync(Guid id, UpdatePacienteDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
