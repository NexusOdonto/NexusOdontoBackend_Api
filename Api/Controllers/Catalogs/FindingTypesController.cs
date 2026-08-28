using Api.Filters;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.FindingType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class FindingTypesController(IFindingTypeService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("CATALOGS", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<FindingTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<FindingTypeDto>>> List() => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [RequirePermission("CATALOGS", PermissionAction.View)]
    [ProducesResponseType(typeof(FindingTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FindingTypeDto>> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequirePermission("CATALOGS", PermissionAction.Create)]
    [ProducesResponseType(typeof(FindingTypeDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<FindingTypeDto>> Create([FromBody] CreateFindingTypeRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("CATALOGS", PermissionAction.Edit)]
    [ProducesResponseType(typeof(FindingTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FindingTypeDto>> Update(Guid id, [FromBody] UpdateFindingTypeRequestDto request)
    {
        var updated = await service.UpdateAsync(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("CATALOGS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id) => await service.DeleteAsync(id) ? NoContent() : NotFound();
}
