using Api.Filters;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers.Common;

[ApiController]
[Authorize]
public abstract class CrudControllerBase<TService, TResponse, TCreate, TUpdate>(TService service) : ControllerBase
    where TService : ICrudService<TResponse, TCreate, TUpdate>
{
    protected TService Service { get; } = service;

    protected abstract string PermissionModule { get; }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public virtual async Task<ActionResult<IReadOnlyList<TResponse>>> List()
    {
        if (!await HasPermissionAsync(PermissionAction.View)) return Forbid();
        return Ok(await Service.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public virtual async Task<ActionResult<TResponse>> GetById(Guid id)
    {
        if (!await HasPermissionAsync(PermissionAction.View)) return Forbid();
        var item = await Service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public virtual async Task<ActionResult<TResponse>> Create([FromBody] TCreate request)
    {
        if (!await HasPermissionAsync(PermissionAction.Create)) return Forbid();
        var created = await Service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = GetResourceId(created) }, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public virtual async Task<ActionResult<TResponse>> Update(Guid id, [FromBody] TUpdate request)
    {
        if (!await HasPermissionAsync(PermissionAction.Edit)) return Forbid();
        var updated = await Service.UpdateAsync(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        if (!await HasPermissionAsync(PermissionAction.Delete)) return Forbid();
        return await Service.DeleteAsync(id) ? NoContent() : NotFound();
    }

    protected virtual Guid GetResourceId(TResponse response)
    {
        var idProperty = typeof(TResponse).GetProperty("Id")
            ?? throw new InvalidOperationException($"Type {typeof(TResponse).Name} must expose an Id property.");

        return idProperty.GetValue(response) is Guid id
            ? id
            : throw new InvalidOperationException($"Type {typeof(TResponse).Name}.Id must be a Guid.");
    }

    private async Task<bool> HasPermissionAsync(PermissionAction action)
    {
        var userRepository = HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var principal = HttpContext.User;

        if (principal.Identity?.IsAuthenticated != true) return false;

        var userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!Guid.TryParse(userIdClaim, out var userId)) return false;

        var user = await userRepository.GetByIdWithRolesAsync(userId, HttpContext.RequestAborted);
        if (user is null) return false;

        var normalizedModule = PermissionModule.Trim().ToUpperInvariant();
        var requiredActionCode = action switch
        {
            PermissionAction.Create => "CREATE",
            PermissionAction.Edit => "EDIT",
            PermissionAction.View => "VIEW",
            PermissionAction.Delete => "DELETE",
            _ => null
        };

        if (requiredActionCode is null) return false;

        return user.UserRoles
            .SelectMany(ur => ur.RoleEntity?.Permissions ?? [])
            .Any(permission =>
                string.Equals(permission.Module, normalizedModule, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(
                    permission.ActionPermissionEntity?.Code,
                    requiredActionCode,
                    StringComparison.OrdinalIgnoreCase));
    }
}
