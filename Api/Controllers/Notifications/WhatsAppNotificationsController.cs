using Api.Filters;
using Application.Contracts.Services.Notifications;
using Application.DTOs.Notifications.WhatsAppNotification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Notifications;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class WhatsAppNotificationsController(IWhatsAppNotificationService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("NOTIFICATIONS", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<WhatsAppNotificationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<WhatsAppNotificationResponseDto>>> List()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [RequirePermission("NOTIFICATIONS", PermissionAction.View)]
    [ProducesResponseType(typeof(WhatsAppNotificationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WhatsAppNotificationResponseDto>> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequirePermission("NOTIFICATIONS", PermissionAction.Create)]
    [ProducesResponseType(typeof(WhatsAppNotificationResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<WhatsAppNotificationResponseDto>> Create([FromBody] CreateWhatsAppNotificationRequestDto request)
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
