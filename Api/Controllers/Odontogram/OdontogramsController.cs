using Api.Controllers.Common;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.Odontogram;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Odontogram;

[Route("api/v1/[controller]")]
public sealed class OdontogramsController(IOdontogramService service)
    : CrudControllerBase<IOdontogramService, OdontogramResponseDto, CreateOdontogramRequestDto, UpdateOdontogramRequestDto>(service)
{
    protected override string PermissionModule => "ODONTOGRAM";
}
