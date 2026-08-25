using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public enum PermissionAction
{
    Create,
    Edit,
    View,
    Delete
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RequirePermissionAttribute : TypeFilterAttribute
{
    public RequirePermissionAttribute(string module, PermissionAction action)
        : base(typeof(RequirePermissionFilter))
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(module);
        Arguments = [module, action];
    }
}

public sealed class RequirePermissionFilter(
    string module,
    PermissionAction action,
    IUsuarioRepository usuarioRepository) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var principal = context.HttpContext.User;

        if (principal.Identity?.IsAuthenticated != true)
        {
            context.Result = new ChallengeResult();
            return;
        }

        var userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var roleIdClaim = principal.FindFirstValue("rolId");

        if (!Guid.TryParse(userIdClaim, out var userId) ||
            !Guid.TryParse(roleIdClaim, out var roleId))
        {
            context.Result = new ForbidResult();
            return;
        }

        var usuario = await usuarioRepository.GetByIdWithRoleAsync(
            userId,
            context.HttpContext.RequestAborted);

        if (usuario is null || usuario.RolId != roleId || usuario.Rol is null)
        {
            context.Result = new ForbidResult();
            return;
        }

        var normalizedModule = module.Trim().ToUpperInvariant();
        var permiso = usuario.Rol.Permisos.FirstOrDefault(permission =>
            string.Equals(
                permission.Modulo.Value,
                normalizedModule,
                StringComparison.Ordinal));

        var isAllowed = permiso is not null && action switch
        {
            PermissionAction.Create => permiso.PuedeCrear,
            PermissionAction.Edit => permiso.PuedeEditar,
            PermissionAction.View => permiso.PuedeVer,
            PermissionAction.Delete => permiso.PuedeEliminar,
            _ => false
        };

        if (!isAllowed)
        {
            context.Result = new ForbidResult();
        }
    }
}
