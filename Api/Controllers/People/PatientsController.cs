using Api.Controllers.Common;
using Api.Filters;
using Application.Contracts.Services.People;
using Application.DTOs.Patients;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.People;

[Route("api/v1/[controller]")]
public sealed class PatientsController(
    IPatientService service,
    IPatientOnboardingService onboardingService)
    : CrudControllerBase<IPatientService, PatientResponseDto, CreatePatientDto, UpdatePatientDto>(service)
{
    protected override string PermissionModule => "PATIENTS";

    [HttpPost("onboard")]
    [RequirePermission("PATIENTS", PermissionAction.Create)]
    [ProducesResponseType(typeof(OnboardPatientResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<OnboardPatientResponseDto>> Onboard(
        [FromBody] OnboardPatientDto request,
        CancellationToken cancellationToken)
    {
        var result = await onboardingService.OnboardAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.PatientId }, result);
    }
}
