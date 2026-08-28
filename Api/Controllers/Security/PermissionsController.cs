using Api.Controllers.Common;
using Application.Contracts.Services.Security;
using Application.DTOs.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Security;

[Route("api/v1/[controller]")]
public sealed class PermissionsController(IPermissionService service)
    : CrudControllerBase<IPermissionService, PermissionResponseDto, CreatePermissionDto, UpdatePermissionDto>(service)
{
    protected override string PermissionModule => "PERMISSIONS";
}
