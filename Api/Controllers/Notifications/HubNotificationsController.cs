using Api.Filters;
using Application.Contracts.Services.Notifications;
using Application.DTOs.Notifications.HubNotification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers.Notifications;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class HubNotificationsController(IHubNotificationService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("NOTIFICATIONS", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<HubNotificationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<HubNotificationResponseDto>>> List()
        => Ok(await service.GetAllAsync());

    /// <summary>Recent notifications addressed to the signed-in user or to one of their roles.</summary>
    [HttpGet("mine")]
    [ProducesResponseType(typeof(IReadOnlyList<HubNotificationResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<HubNotificationResponseDto>>> Mine(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();
        return Ok(await service.GetMineAsync(userId, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("NOTIFICATIONS", PermissionAction.View)]
    [ProducesResponseType(typeof(HubNotificationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HubNotificationResponseDto>> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequirePermission("NOTIFICATIONS", PermissionAction.Create)]
    [ProducesResponseType(typeof(HubNotificationResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<HubNotificationResponseDto>> Create([FromBody] CreateHubNotificationRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("NOTIFICATIONS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
        => await service.DeleteAsync(id) ? NoContent() : NotFound();
}
