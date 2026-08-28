using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.WhatsAppNotificationStatus;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class WhatsAppNotificationStatusesController(IWhatsAppNotificationStatusService service)
    : CatalogCrudControllerBase<IWhatsAppNotificationStatusService, WhatsAppNotificationStatusDto>(service);
