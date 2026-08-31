using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Contracts.Repositories.People;
using Application.Services.Security;
using Api.Filters;
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
    IUserRepository userRepository) : IAsyncAuthorizationFilter
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

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            context.Result = new ForbidResult();
            return;
        }

        var user = await userRepository.GetByIdWithRolesAsync(
            userId,
            context.HttpContext.RequestAborted);

        if (user is null)
        {
            context.Result = new ForbidResult();
            return;
        }

        var requiredActionCode = action switch
        {
            PermissionAction.Create => "CREATE",
            PermissionAction.Edit => "EDIT",
            PermissionAction.View => "VIEW",
            PermissionAction.Delete => "DELETE",
            _ => null
        };

        if (requiredActionCode is null ||
            !PermissionChecker.HasPermission(user, module, requiredActionCode))
        {
            context.Result = new ForbidResult();
        }
    }
}
