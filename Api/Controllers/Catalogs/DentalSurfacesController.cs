using Api.Filters;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.DentalSurface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class DentalSurfacesController(IDentalSurfaceService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("CATALOGS", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<DentalSurfaceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<DentalSurfaceDto>>> List() => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [RequirePermission("CATALOGS", PermissionAction.View)]
    [ProducesResponseType(typeof(DentalSurfaceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DentalSurfaceDto>> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequirePermission("CATALOGS", PermissionAction.Create)]
    [ProducesResponseType(typeof(DentalSurfaceDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<DentalSurfaceDto>> Create([FromBody] CreateDentalSurfaceRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("CATALOGS", PermissionAction.Edit)]
    [ProducesResponseType(typeof(DentalSurfaceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DentalSurfaceDto>> Update(Guid id, [FromBody] UpdateDentalSurfaceRequestDto request)
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
