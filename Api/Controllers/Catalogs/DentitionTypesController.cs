using Api.Filters;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.DentitionType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class DentitionTypesController(IDentitionTypeService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("CATALOGS", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<DentitionTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<DentitionTypeDto>>> List() => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [RequirePermission("CATALOGS", PermissionAction.View)]
    [ProducesResponseType(typeof(DentitionTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DentitionTypeDto>> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequirePermission("CATALOGS", PermissionAction.Create)]
    [ProducesResponseType(typeof(DentitionTypeDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<DentitionTypeDto>> Create([FromBody] CreateDentitionTypeRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("CATALOGS", PermissionAction.Edit)]
    [ProducesResponseType(typeof(DentitionTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DentitionTypeDto>> Update(Guid id, [FromBody] UpdateDentitionTypeRequestDto request)
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
