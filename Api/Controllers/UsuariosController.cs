using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTOs.Auth;
using Application.DTOs.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/usuarios")]
    public sealed class UsuariosController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UsuariosController(
            IAuthService authService,
            IUsuarioRepository usuarioRepository,
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> Crear(
            [FromBody] RegisterRequestDto request,
            CancellationToken cancellationToken)
        {
            var usuario = await _authService.RegisterAsync(request, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, usuario);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UsuarioResponseDto>>> Listar(
            CancellationToken cancellationToken)
        {
            var usuarios = await _usuarioRepository.GetAllWithRoleAsync(cancellationToken);
            var response = usuarios
                .Select(usuario => new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre.Value,
                    Correo = usuario.Correo.Value,
                    RolId = usuario.RolId
                })
                .ToList();

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UsuarioResponseDto>> Editar(
            Guid id,
            [FromBody] UpdateUsuarioRequestDto request,
            CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdWithRoleAsync(id, cancellationToken);

            if (usuario is null)
            {
                return NotFound();
            }

            usuario.CambiarNombre(request.Nombre);
            usuario.CambiarCorreo(request.Correo);
            usuario.CambiarRol(request.RolId);
            usuario.FechaActualizacion = DateTime.UtcNow;

            _usuarioRepository.Update(usuario);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre.Value,
                Correo = usuario.Correo.Value,
                RolId = usuario.RolId
            });
        }

        [HttpPatch("{id:guid}/desactivar")]
        public async Task<IActionResult> Desactivar(
            Guid id,
            CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdWithRoleAsync(id, cancellationToken);

            if (usuario is null)
            {
                return NotFound();
            }

            usuario.Activo = false;
            usuario.FechaActualizacion = DateTime.UtcNow;

            _usuarioRepository.Update(usuario);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
