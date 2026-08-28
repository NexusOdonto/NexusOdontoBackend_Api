using Api.Controllers.Common;
using Application.Contracts.Services.People;
using Application.DTOs.Employees;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.People;

[Route("api/v1/[controller]")]
public sealed class EmployeesController(IEmployeeService service)
    : CrudControllerBase<IEmployeeService, EmployeeResponseDto, CreateEmployeeDto, UpdateEmployeeDto>(service)
{
    protected override string PermissionModule => "PEOPLE";
}
