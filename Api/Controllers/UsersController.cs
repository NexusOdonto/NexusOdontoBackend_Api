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
        var UserEntity = await _authService.RegisterAsync(request, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, UserEntity);
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
        var UserEntity = await _userRepository.GetByIdWithRolesAsync(id, cancellationToken);

        if (UserEntity is null)
        {
            return NotFound();
        }

        UserEntity.UserRoles.Clear();
        UserEntity.UserRoles.Add(new UserRoleEntity(UserEntity.Id, request.RoleId));
        UserEntity.IsActive = request.IsActive;
        UserEntity.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(UserEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Ok(AuthService.MapToResponse(UserEntity));
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var UserEntity = await _userRepository.GetByIdWithRolesAsync(id, cancellationToken);

        if (UserEntity is null)
        {
            return NotFound();
        }

        UserEntity.Deactivate();

        _userRepository.Update(UserEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
