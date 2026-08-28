using Api.Filters;
using Application.Contracts.Services.Audit;
using Application.DTOs.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Audit;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class AuditEventsController(IAuditEventService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("AUDIT", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<AuditEventResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AuditEventResponseDto>>> List()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [RequirePermission("AUDIT", PermissionAction.View)]
    [ProducesResponseType(typeof(AuditEventResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuditEventResponseDto>> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequirePermission("AUDIT", PermissionAction.Create)]
    [ProducesResponseType(typeof(AuditEventResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<AuditEventResponseDto>> Create([FromBody] CreateAuditEventRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("AUDIT", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
        => await service.DeleteAsync(id) ? NoContent() : NotFound();
}
