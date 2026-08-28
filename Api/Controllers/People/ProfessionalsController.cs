using Api.Controllers.Common;
using Application.Contracts.Services.People;
using Application.DTOs.Professionals;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.People;

[Route("api/v1/[controller]")]
public sealed class ProfessionalsController(IProfessionalService service)
    : CrudControllerBase<IProfessionalService, ProfessionalResponseDto, CreateProfessionalDto, UpdateProfessionalDto>(service)
{
    protected override string PermissionModule => "PEOPLE";
}
