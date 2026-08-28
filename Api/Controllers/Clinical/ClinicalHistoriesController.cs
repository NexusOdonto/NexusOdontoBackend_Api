using Api.Filters;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.ClinicalHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Clinical;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class ClinicalHistoriesController(IClinicalHistoryService service) : ControllerBase
{
    [HttpGet]
    [RequirePermission("CLINICAL", PermissionAction.View)]
    [ProducesResponseType(typeof(IReadOnlyList<ClinicalHistoryResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ClinicalHistoryResponseDto>>> List()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [RequirePermission("CLINICAL", PermissionAction.View)]
    [ProducesResponseType(typeof(ClinicalHistoryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClinicalHistoryResponseDto>> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("CLINICAL", PermissionAction.Edit)]
    [ProducesResponseType(typeof(ClinicalHistoryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClinicalHistoryResponseDto>> Update(Guid id, [FromBody] UpdateClinicalHistoryRequestDto request)
    {
        var updated = await service.UpdateAsync(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("CLINICAL", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
        => await service.DeleteAsync(id) ? NoContent() : NotFound();
}
