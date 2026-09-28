using Api.Controllers.Common;
using Api.Filters;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Appointments;
using Application.DTOs.Availabilities;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers.Schedule;

[Route("api/v1/[controller]")]
public sealed class AvailabilitiesController(IAvailabilityService service)
    : CrudControllerBase<IAvailabilityService, AvailabilityResponseDto, CreateAvailabilityDto, UpdateAvailabilityDto>(service)
{
    protected override string PermissionModule => "APPOINTMENTS";

    [HttpPost("impact")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<AffectedAppointmentDto>>> Impact(
        [FromBody] AvailabilityImpactRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!await HasPermissionAsync(PermissionAction.View)) return Forbid();
        return Ok(await Service.GetImpactAsync(request, cancellationToken));
    }

    [HttpPut("professional/{professionalId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReplaceProfessionalAvailabilityResultDto>> ReplaceForProfessional(
        Guid professionalId,
        [FromBody] ReplaceProfessionalAvailabilityDto request,
        CancellationToken cancellationToken)
    {
        if (!await HasPermissionAsync(PermissionAction.Edit)) return Forbid();

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        Guid? userId = Guid.TryParse(userIdClaim, out var uid) ? uid : null;

        // The admin's HTTP request must not abort in-flight bot notifications once the schedule is saved.
        return Ok(await Service.ReplaceForProfessionalAsync(professionalId, request, userId, CancellationToken.None));
    }
}
