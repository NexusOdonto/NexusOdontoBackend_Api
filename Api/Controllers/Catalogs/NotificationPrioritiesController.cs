using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.NotificationPriority;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class NotificationPrioritiesController(INotificationPriorityService service)
    : CatalogCrudControllerBase<INotificationPriorityService, NotificationPriorityDto>(service);
