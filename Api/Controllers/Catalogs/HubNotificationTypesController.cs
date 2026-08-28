using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.HubNotificationType;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class HubNotificationTypesController(IHubNotificationTypeService service)
    : CatalogCrudControllerBase<IHubNotificationTypeService, HubNotificationTypeDto>(service);
