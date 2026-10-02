using Api.Controllers.Common;
using Api.Filters;
using Application.Contracts.Services.People;
using Application.DTOs.People;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.People;

[Route("api/v1/[controller]")]
public sealed class PersonsController(IPersonService service)
    : CrudControllerBase<IPersonService, PersonResponseDto, CreatePersonDto, UpdatePersonDto>(service)
{
    protected override string PermissionModule => "PEOPLE";

    /// <summary>
    /// Lookup person by document number (cédula) — the unique identity key.
    /// </summary>
    [HttpGet("by-document/{documentNumber}")]
    [RequirePermission("PEOPLE", PermissionAction.View)]
    [ProducesResponseType(typeof(PersonResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonResponseDto>> GetByDocument(
        string documentNumber,
        CancellationToken cancellationToken)
    {
        var person = await Service.GetByDocumentNumberAsync(documentNumber, cancellationToken);
        return person is null ? NotFound() : Ok(person);
    }
}
