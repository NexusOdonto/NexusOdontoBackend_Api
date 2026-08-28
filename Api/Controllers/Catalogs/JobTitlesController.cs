using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.JobTitle;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class JobTitlesController(IJobTitleService service)
    : CatalogCrudControllerBase<IJobTitleService, JobTitleDto>(service);
