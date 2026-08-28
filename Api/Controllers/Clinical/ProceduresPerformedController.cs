using Api.Filters;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.ProcedurePerformed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Clinical;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class ProceduresPerformedController(IProcedurePerformedService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("CLINICAL", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<ProcedurePerformedResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProcedurePerformedResponseDto>>> List()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [RequirePermission("CLINICAL", PermissionAction.View)]
    [ProducesResponseType(typeof(ProcedurePerformedResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProcedurePerformedResponseDto>> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequirePermission("CLINICAL", PermissionAction.Create)]
    [ProducesResponseType(typeof(ProcedurePerformedResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ProcedurePerformedResponseDto>> Create([FromBody] CreateProcedurePerformedRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("CLINICAL", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
        => await service.DeleteAsync(id) ? NoContent() : NotFound();
}
