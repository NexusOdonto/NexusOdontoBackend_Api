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

    /// <summary>
    /// Lookup the canonical patient for a document number (cédula).
    /// </summary>
    [HttpGet("by-document/{documentNumber}")]
    [RequirePermission("PATIENTS", PermissionAction.View)]
    [ProducesResponseType(typeof(PatientResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientResponseDto>> GetByDocument(
        string documentNumber,
        CancellationToken cancellationToken)
    {
        var patient = await service.GetByDocumentNumberAsync(documentNumber, cancellationToken);
        return patient is null ? NotFound() : Ok(patient);
    }

    /// <summary>
    /// Lookup patient by person id without scanning the full patients list.
    /// </summary>
    [HttpGet("by-person/{personId:guid}")]
    [RequirePermission("PATIENTS", PermissionAction.View)]
    [ProducesResponseType(typeof(PatientResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientResponseDto>> GetByPerson(
        Guid personId,
        CancellationToken cancellationToken)
    {
        var patient = await service.GetByPersonIdAsync(personId, cancellationToken);
        return patient is null ? NotFound() : Ok(patient);
    }

    [HttpPost("onboard")]
    [RequirePermission("PATIENTS", PermissionAction.Create)]
    [ProducesResponseType(typeof(OnboardPatientResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<OnboardPatientResponseDto>> Onboard(
        [FromBody] OnboardPatientDto request,
        CancellationToken cancellationToken)
    {
        var result = await onboardingService.OnboardAsync(request, cancellationToken);
        if (result.AlreadyExisted)
            return Ok(result);

        return CreatedAtAction(nameof(GetById), new { id = result.PatientId }, result);
    }
}
