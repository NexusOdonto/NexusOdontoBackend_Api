using Application.Contracts.Repositories;
using Application.DTOs;
using Application.Services;
using Domain.Entities.People;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class PatientsController : ControllerBase
{
    private readonly IRepository<Patient> _patientRepository;
    private readonly PatientService _patientService;

    public PatientsController(
        IRepository<Patient> patientRepository,
        PatientService patientService)
    {
        _patientRepository = patientRepository;
        _patientService = patientService;
    }

    [HttpGet]
    [Authorize(Policy = "PATIENTS.VIEW")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<Patient>>> List()
    {
        var patients = await _patientRepository.GetAllAsync();

        return Ok(patients);
    }

    [HttpPost]
    [Authorize(Policy = "PATIENTS.CREATE")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePatientRequestDto request,
        CancellationToken cancellationToken)
    {
        var patientId = await _patientService.CreateAsync(request, cancellationToken);

        return Created(
            $"/api/v1/patients/{patientId}",
            new { patientId });
    }
}
