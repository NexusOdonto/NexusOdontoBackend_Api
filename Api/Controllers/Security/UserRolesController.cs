using Api.Filters;
using Application.Contracts.Services.Security;
using Application.DTOs.UserRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Security;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class UserRolesController(IUserRoleService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("USERS", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<UserRoleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserRoleDto>>> List()
        => Ok(await service.GetAllAsync());

    [HttpGet("users/{userId:guid}/roles")]
    [RequirePermission("USERS", PermissionAction.View)]
    [ProducesResponseType(typeof(UserRoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserRoleDto>> GetByUserId(Guid userId)
    {
        var item = await service.GetByIdAsync(userId);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("users/{userId:guid}/roles/{roleId:guid}")]
    [RequirePermission("USERS", PermissionAction.Create)]
    [ProducesResponseType(typeof(UserRoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserRoleDto>> Assign(Guid userId, Guid roleId)
    {
        var created = await service.CreateAsync(new UserRoleDto { UserId = userId, RoleId = roleId });
        return CreatedAtAction(nameof(GetByUserId), new { userId }, created);
    }

    [HttpDelete("users/{userId:guid}/roles/{roleId:guid}")]
    [RequirePermission("USERS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(Guid userId, Guid roleId)
    {
        var assignments = await service.GetAllAsync();
        if (assignments.All(a => a.UserId != userId || a.RoleId != roleId))
        {
            return NotFound();
        }

        return await service.DeleteAsync(userId) ? NoContent() : NotFound();
    }
}
