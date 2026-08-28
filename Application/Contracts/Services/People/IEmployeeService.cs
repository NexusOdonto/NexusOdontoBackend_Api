using Application.DTOs.Employees;

namespace Application.Contracts.Services.People
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateEmployeeDto request);
        Task<object?> UpdateAsync(Guid id, UpdateEmployeeDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
