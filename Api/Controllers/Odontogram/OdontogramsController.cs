using Api.Controllers.Common;
using Application.Contracts.Services.Odontogram;
using Application.DTOs.Odontogram.FullOdontogram;
using Application.DTOs.Odontogram.Odontogram;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Odontogram;

[Route("api/v1/[controller]")]
public sealed class OdontogramsController(IOdontogramService service)
    : CrudControllerBase<IOdontogramService, OdontogramResponseDto, CreateOdontogramRequestDto, UpdateOdontogramRequestDto>(service)
{
    protected override string PermissionModule => "ODONTOGRAM";

    [HttpPost("full")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FullOdontogramResponseDto>> SaveFull(
        [FromBody] SaveFullOdontogramRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await Service.SaveFullOdontogramAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("patient/{patientId:guid}/latest")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullOdontogramResponseDto>> GetLatestByPatient(
        Guid patientId,
        CancellationToken cancellationToken)
    {
        var result = await Service.GetLatestByPatientIdAsync(patientId, cancellationToken);
        if (result is null)
        {
            return NotFound(new { message = $"No se encontró un odontograma registrado para el paciente con ID '{patientId}'." });
        }
        return Ok(result);
    }

    [HttpGet("{id:guid}/full")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullOdontogramResponseDto>> GetFullById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await Service.GetFullByIdAsync(id, cancellationToken);
        if (result is null)
        {
            return NotFound(new { message = $"No se encontró el odontograma con ID '{id}'." });
        }
        return Ok(result);
    }
}
