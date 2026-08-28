using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.SessionStatus;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class SessionStatusesController(ISessionStatusService service)
    : CatalogCrudControllerBase<ISessionStatusService, SessionStatusDto>(service);
