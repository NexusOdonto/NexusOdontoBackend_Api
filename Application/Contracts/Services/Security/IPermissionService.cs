using Application.DTOs.Permissions;

namespace Application.Contracts.Services.Security
{
    public interface IPermissionService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreatePermissionDto request);
        Task<object?> UpdateAsync(Guid id, UpdatePermissionDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
