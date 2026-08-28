using Application.Contracts.Services.Common;
using Application.DTOs.Roles;

namespace Application.Contracts.Services.Security
{
    public interface IRoleService : ICrudService<RoleResponseDto, CreateRoleDto, UpdateRoleDto>
    {
    }
}
