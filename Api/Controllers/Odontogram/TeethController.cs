using Api.Filters;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.Tooth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Odontogram;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class TeethController(IToothService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("ODONTOGRAM", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<ToothResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ToothResponseDto>>> List()
        => Ok(await service.GetAllAsync());

    [HttpGet("{code}")]
    [RequirePermission("ODONTOGRAM", PermissionAction.View)]
    [ProducesResponseType(typeof(ToothResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ToothResponseDto>> GetByCode(string code)
    {
        var item = await service.GetByCodeAsync(code);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [RequirePermission("ODONTOGRAM", PermissionAction.Create)]
    [ProducesResponseType(typeof(ToothResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ToothResponseDto>> Create([FromBody] CreateToothRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetByCode), new { code = created.Code }, created);
    }

    [HttpPut("{code}")]
    [RequirePermission("ODONTOGRAM", PermissionAction.Edit)]
    [ProducesResponseType(typeof(ToothResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ToothResponseDto>> Update(string code, [FromBody] UpdateToothRequestDto request)
    {
        var updated = await service.UpdateAsync(code, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{code}")]
    [RequirePermission("ODONTOGRAM", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string code)
        => await service.DeleteAsync(code) ? NoContent() : NotFound();
}
