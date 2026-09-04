using Api.Controllers.Common;
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
}
