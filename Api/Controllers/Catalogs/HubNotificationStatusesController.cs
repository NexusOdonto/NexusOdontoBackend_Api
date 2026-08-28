using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.HubNotificationStatus;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class HubNotificationStatusesController(IHubNotificationStatusService service)
    : CatalogCrudControllerBase<IHubNotificationStatusService, HubNotificationStatusDto>(service);
