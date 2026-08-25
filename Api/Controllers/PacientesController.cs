using Application.Contracts.Repositories;
using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public sealed class PacientesController : ControllerBase
    {
        private readonly IRepository<Paciente> _pacienteRepository;
        private readonly PacienteService _pacienteService;

        public PacientesController(
            IRepository<Paciente> pacienteRepository,
            PacienteService pacienteService)
        {
            _pacienteRepository = pacienteRepository;
            _pacienteService = pacienteService;
        }

        [HttpGet]
        [Authorize(Policy = "PACIENTES.VER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IReadOnlyList<Paciente>>> Listar()
        {
            var pacientes = await _pacienteRepository.GetAllAsync();

            return Ok(pacientes);
        }

        [HttpPost]
        [Authorize(Policy = "PACIENTES.CREAR")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Crear(
            [FromBody] CrearPacienteRequestDto request,
            CancellationToken cancellationToken)
        {
            var pacienteId = await _pacienteService.CrearAsync(request, cancellationToken);

            return Created(
                $"/api/v1/pacientes/{pacienteId}",
                new { pacienteId });
        }
    }
}
