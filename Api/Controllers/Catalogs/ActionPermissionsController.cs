using Api.Controllers.Common;
using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.ActionPermission;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalogs;

[Route("api/v1/[controller]")]
public sealed class ActionPermissionsController(IActionPermissionService service)
    : CatalogCrudControllerBase<IActionPermissionService, ActionPermissionDto>(service);
