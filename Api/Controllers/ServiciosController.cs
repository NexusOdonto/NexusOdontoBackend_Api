using Application.Contracts.Repositories;
using Application.DTOs;
using Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public sealed class ServiciosController : ControllerBase
    {
        private readonly IRepository<Servicio> _servicioRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiciosController(
            IRepository<Servicio> servicioRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _servicioRepository = servicioRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "CATALOGOS.VER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IReadOnlyList<Servicio>>> Listar()
        {
            var servicios = await _servicioRepository.GetAllAsync();

            return Ok(servicios);
        }

        [HttpPost]
        [Authorize(Policy = "CATALOGOS.CREAR")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Crear(
            [FromBody] CrearServicioRequestDto request,
            CancellationToken cancellationToken)
        {
            var servicio = _mapper.Map<Servicio>(request);

            await _servicioRepository.AddAsync(servicio);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Created(
                $"/api/v1/servicios/{servicio.Id}",
                new { servicioId = servicio.Id });
        }
    }
}
