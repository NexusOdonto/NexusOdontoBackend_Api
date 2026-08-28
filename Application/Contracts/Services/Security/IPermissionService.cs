using Application.Contracts.Services.Common;
using Application.DTOs.Permissions;

namespace Application.Contracts.Services.Security
{
    public interface IPermissionService : ICrudService<PermissionResponseDto, CreatePermissionDto, UpdatePermissionDto>
    {
    }
}
