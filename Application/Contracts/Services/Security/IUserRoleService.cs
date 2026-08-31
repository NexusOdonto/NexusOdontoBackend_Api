using Application.DTOs.UserRoles;

namespace Application.Contracts.Services.Security
{
    public interface IUserRoleService
    {
        Task<IReadOnlyList<UserRoleDto>> GetAllAsync();
        Task<UserRoleDto?> GetByIdAsync(Guid id);
        Task<UserRoleDto> CreateAsync(UserRoleDto request);
        Task<bool> DeleteAsync(Guid userId, Guid roleId);
    }
}
