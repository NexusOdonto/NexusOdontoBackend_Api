using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.AppointmentOrigin;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class AppointmentOriginsController(IAppointmentOriginService service)
    : CatalogCrudControllerBase<IAppointmentOriginService, AppointmentOriginDto>(service);
