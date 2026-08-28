using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.SessionStatus;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class SessionStatusService(CatalogServiceCore<SessionStatusEntity> core) : ISessionStatusService
{
    public Task<IReadOnlyList<SessionStatusDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToSessionStatusDto);

    public Task<SessionStatusDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToSessionStatusDto);

    public Task<SessionStatusDto> CreateAsync(SessionStatusDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new SessionStatusEntity(request.Code, request.Name, request.Description, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToSessionStatusDto);
    }

    public Task<SessionStatusDto?> UpdateAsync(Guid id, SessionStatusDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToSessionStatusDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
