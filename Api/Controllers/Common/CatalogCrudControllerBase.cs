using Api.Filters;
using Application.Contracts.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Common;

[ApiController]
[Authorize]
public abstract class CatalogCrudControllerBase<TService, TDto>(TService service) : ControllerBase
    where TService : ICatalogCrudService<TDto>
{
    protected TService Service { get; } = service;

    [HttpGet]
    [RequirePermission("CATALOGS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public virtual async Task<ActionResult<IReadOnlyList<TDto>>> List()
        => Ok(await Service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [RequirePermission("CATALOGS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public virtual async Task<ActionResult<TDto>> GetById(Guid id)
    {
        var item = await Service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequirePermission("CATALOGS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TDto request)
    {
        var created = await Service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = GetResourceId(created) }, created);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("CATALOGS", PermissionAction.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public virtual async Task<ActionResult<TDto>> Update(Guid id, [FromBody] TDto request)
    {
        var updated = await Service.UpdateAsync(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("CATALOGS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public virtual async Task<IActionResult> Delete(Guid id)
        => await Service.DeleteAsync(id) ? NoContent() : NotFound();

    protected virtual Guid GetResourceId(TDto response)
    {
        var idProperty = typeof(TDto).GetProperty("Id")
            ?? throw new InvalidOperationException($"Type {typeof(TDto).Name} must expose an Id property.");

        return idProperty.GetValue(response) is Guid id
            ? id
            : throw new InvalidOperationException($"Type {typeof(TDto).Name}.Id must be a Guid.");
    }
}
