using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.AppointmentStatus;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class AppointmentStatusesController(IAppointmentStatusService service)
    : CatalogCrudControllerBase<IAppointmentStatusService, AppointmentStatusDto>(service);
