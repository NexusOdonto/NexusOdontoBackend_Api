using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services;
using Application.DTOs.Auth;
using Application.DTOs.Users;
using Application.Services;
using Domain.Entities.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(
        IAuthService authService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> Create(
        [FromBody] RegisterRequestDto request,
        CancellationToken cancellationToken)
    {
        var user = await _authService.RegisterAsync(request, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, user);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserResponseDto>>> List(
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllWithRolesAsync(cancellationToken);
        var response = users
            .Select(AuthService.MapToResponse)
            .ToList();

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResponseDto>> Edit(
        Guid id,
        [FromBody] UpdateUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdWithRolesAsync(id, cancellationToken);

        if (user is null)
        {
            return NotFound();
        }

        user.UserRoles.Clear();
        user.UserRoles.Add(new UserRoleEntity(user.Id, request.RoleId));
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Ok(AuthService.MapToResponse(user));
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdWithRolesAsync(id, cancellationToken);

        if (user is null)
        {
            return NotFound();
        }

        user.Deactivate();

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
