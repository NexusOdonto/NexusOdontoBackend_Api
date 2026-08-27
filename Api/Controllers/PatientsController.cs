using Application.Contracts.Repositories;
using Application.Contracts.Services.People;
using Application.DTOs;
using Application.DTOs.Pacientes;
using Application.DTOs.Personas;
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
    private readonly IPersonService _personService;
    private readonly IPatientService _patientService;

    public PatientsController(
        IRepository<Patient> patientRepository,
        IPersonService personService,
        IPatientService patientService)
    {
        _patientRepository = patientRepository;
        _personService = personService;
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
        [FromBody] CreatePatientRequestDto request)
    {
        var person = await _personService.CreateAsync(new CreatePersonaDto
        {
            TipoDocumentoId = request.DocumentTypeId,
            NumeroDocumento = request.DocumentNumber,
            Nombres = request.FirstName,
            Apellidos = request.LastName,
            FechaNacimiento = request.DateOfBirth,
            SexoId = request.SexId,
            Telefono = request.Phone,
            Email = request.Email,
            Direccion = request.Address
        });

        var personId = (Guid)person.GetType().GetProperty("Id")!.GetValue(person)!;

        var patient = await _patientService.CreateAsync(new CreatePacienteDto
        {
            PersonaId = personId,
            ContactoEmergencia = request.EmergencyContact,
            TelefonoEmergencia = request.EmergencyPhone
        });

        var patientId = (Guid)patient.GetType().GetProperty("Id")!.GetValue(patient)!;

        return Created(
            $"/api/v1/patients/{patientId}",
            new { patientId });
    }
}
