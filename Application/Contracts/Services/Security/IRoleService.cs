using Application.DTOs.Roles;

namespace Application.Contracts.Services.Security
{
    public interface IRoleService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateRoleDto request);
        Task<object?> UpdateAsync(Guid id, UpdateRoleDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
