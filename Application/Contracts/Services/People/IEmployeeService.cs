using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Empleados;

namespace Application.Contracts.Services.People
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateEmpleadoDto request);
        Task<object?> UpdateAsync(Guid id, UpdateEmpleadoDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
