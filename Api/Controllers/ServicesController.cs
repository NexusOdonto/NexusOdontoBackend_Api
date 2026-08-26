using Application.Contracts.Repositories;
using Application.DTOs;
using Domain.Entities.Catalogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class ServicesController : ControllerBase
{
    private readonly IRepository<Service> _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ServicesController(
        IRepository<Service> serviceRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Authorize(Policy = "CATALOGS.VIEW")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<Service>>> List()
    {
        var services = await _serviceRepository.GetAllAsync();

        return Ok(services);
    }

    [HttpPost]
    [Authorize(Policy = "CATALOGS.CREATE")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(
        [FromBody] CreateServiceRequestDto request,
        CancellationToken cancellationToken)
    {
        var service = new Service(
            request.Code,
            request.Name,
            request.Description,
            request.DurationMinutes,
            request.Price,
            request.SortOrder);

        await _serviceRepository.AddAsync(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Created(
            $"/api/v1/services/{service.Id}",
            new { serviceId = service.Id });
    }
}
