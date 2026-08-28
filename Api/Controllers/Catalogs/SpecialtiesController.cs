using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.Specialty;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class SpecialtiesController(ISpecialtyService service)
    : CatalogCrudControllerBase<ISpecialtyService, SpecialtyDto>(service);
