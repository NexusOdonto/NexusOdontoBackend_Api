using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.WhatsAppNotificationType;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class WhatsAppNotificationTypesController(IWhatsAppNotificationTypeService service)
    : CatalogCrudControllerBase<IWhatsAppNotificationTypeService, WhatsAppNotificationTypeDto>(service);
