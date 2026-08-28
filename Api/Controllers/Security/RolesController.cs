using Api.Controllers.Common;
using Application.Contracts.Services.Security;
using Application.DTOs.Roles;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Security;

[Route("api/v1/[controller]")]
public sealed class RolesController(IRoleService service)
    : CrudControllerBase<IRoleService, RoleResponseDto, CreateRoleDto, UpdateRoleDto>(service)
{
    protected override string PermissionModule => "ROLES";
}
