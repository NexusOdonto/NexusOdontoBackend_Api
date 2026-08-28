using Application.DTOs.Catalogs.ActionPermission;

namespace Application.Contracts.Services.Catalogs
{
    public interface IActionPermissionService
    {
        Task<IReadOnlyList<ActionPermissionDto>> GetAllAsync();
        Task<ActionPermissionDto?> GetByIdAsync(Guid id);
        Task<ActionPermissionDto> CreateAsync(ActionPermissionDto request);
        Task<ActionPermissionDto?> UpdateAsync(Guid id, ActionPermissionDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
