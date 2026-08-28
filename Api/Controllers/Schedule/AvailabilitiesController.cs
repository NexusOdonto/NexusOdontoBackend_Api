using Api.Controllers.Common;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Availabilities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Schedule;

[Route("api/v1/[controller]")]
public sealed class AvailabilitiesController(IAvailabilityService service)
    : CrudControllerBase<IAvailabilityService, AvailabilityResponseDto, CreateAvailabilityDto, UpdateAvailabilityDto>(service)
{
    protected override string PermissionModule => "APPOINTMENTS";
}
