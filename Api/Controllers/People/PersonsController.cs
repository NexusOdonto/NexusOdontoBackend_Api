using Api.Controllers.Common;
using Application.Contracts.Services.People;
using Application.DTOs.Patients;
using Application.DTOs.People;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.People;

[Route("api/v1/[controller]")]
public sealed class PersonsController(IPersonService service)
    : CrudControllerBase<IPersonService, PersonResponseDto, CreatePersonDto, UpdatePersonDto>(service)
{
    protected override string PermissionModule => "PEOPLE";
}
