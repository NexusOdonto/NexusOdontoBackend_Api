using Api.Controllers.Common;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.PatientAntecedent;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers.Clinical;

[Route("api/v1/[controller]")]
public sealed class PatientAntecedentsController(IPatientAntecedentService service)
    : CrudControllerBase<IPatientAntecedentService, PatientAntecedentResponseDto, CreatePatientAntecedentRequestDto, UpdatePatientAntecedentRequestDto>(service)
{
    protected override string PermissionModule => "CLINICAL";

    public override async Task<ActionResult<PatientAntecedentResponseDto>> Create([FromBody] CreatePatientAntecedentRequestDto request)
    {
        if (request.RegisteredByUserId is null || request.RegisteredByUserId == Guid.Empty)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (Guid.TryParse(userIdClaim, out var uid))
            {
                request.RegisteredByUserId = uid;
            }
        }
        return await base.Create(request);
    }
}
