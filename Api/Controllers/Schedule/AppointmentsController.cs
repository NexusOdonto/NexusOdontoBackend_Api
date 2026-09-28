using Api.Controllers.Common;
using Api.Filters;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Appointments;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers.Schedule;

[Route("api/v1/[controller]")]
public sealed class AppointmentsController(IAppointmentService service)
    : CrudControllerBase<IAppointmentService, AppointmentResponseDto, CreateAppointmentDto, UpdateAppointmentDto>(service)
{
    protected override string PermissionModule => "APPOINTMENTS";

    public override async Task<ActionResult<AppointmentResponseDto>> Create([FromBody] CreateAppointmentDto request)
    {
        if (request.CreatedByUserId is null || request.CreatedByUserId == Guid.Empty)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (Guid.TryParse(userIdClaim, out var uid))
            {
                request.CreatedByUserId = uid;
            }
        }
        return await base.Create(request);
    }
    [HttpGet("requires-reschedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<RequiresRescheduleAppointmentDto>>> GetRequiresReschedule(CancellationToken cancellationToken)
    {
        if (!await HasPermissionAsync(PermissionAction.View)) return Forbid();
        return Ok(await Service.GetRequiresRescheduleAsync(cancellationToken));
    }

    [HttpGet("{id:guid}/clinical-context")]
    public async Task<ActionResult<AppointmentClinicalContextDto>> GetClinicalContext(Guid id, CancellationToken cancellationToken)
    {
        var context = await Service.GetClinicalContextAsync(id, cancellationToken);
        if (context is null)
        {
            return NotFound(new { message = $"No se encontró la cita con ID '{id}'." });
        }
        return Ok(context);
    }

    [HttpPost("{id:guid}/no-show")]
    public async Task<ActionResult<AppointmentResponseDto>> MarkAsNoShow(Guid id, [FromBody] MarkNoShowDto? request, CancellationToken cancellationToken)
    {
        var result = await Service.MarkAsNoShowAsync(id, request?.Reason, cancellationToken);
        if (result is null)
        {
            return NotFound(new { message = $"No se encontró la cita con ID '{id}'." });
        }
        return Ok(result);
    }
}

public sealed class MarkNoShowDto
{
    public string? Reason { get; set; }
}
