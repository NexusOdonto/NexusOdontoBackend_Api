using Application.Contracts.Services.Common;
using Application.DTOs.Employees;

namespace Application.Contracts.Services.People
{
    public interface IEmployeeService : ICrudService<EmployeeResponseDto, CreateEmployeeDto, UpdateEmployeeDto>
    {
    }
}
