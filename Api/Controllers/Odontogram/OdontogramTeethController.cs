using Api.Controllers.Common;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.OdontogramTooth;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Odontogram;

[Route("api/v1/[controller]")]
public sealed class OdontogramTeethController(IOdontogramToothService service)
    : CrudControllerBase<IOdontogramToothService, OdontogramToothResponseDto, CreateOdontogramToothRequestDto, UpdateOdontogramToothRequestDto>(service)
{
    protected override string PermissionModule => "ODONTOGRAM";
}
