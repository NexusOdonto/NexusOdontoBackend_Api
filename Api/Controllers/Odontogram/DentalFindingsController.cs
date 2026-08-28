using Api.Controllers.Common;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.DentalFinding;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Odontogram;

[Route("api/v1/[controller]")]
public sealed class DentalFindingsController(IDentalFindingService service)
    : CrudControllerBase<IDentalFindingService, DentalFindingResponseDto, CreateDentalFindingRequestDto, UpdateDentalFindingRequestDto>(service)
{
    protected override string PermissionModule => "ODONTOGRAM";
}
