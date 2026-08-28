using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.AuditEventType;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class AuditEventTypeService(CatalogServiceCore<AuditEventTypeEntity> core) : IAuditEventTypeService
{
    public Task<IReadOnlyList<AuditEventTypeDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToAuditEventTypeDto);

    public Task<AuditEventTypeDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToAuditEventTypeDto);

    public Task<AuditEventTypeDto> CreateAsync(CreateAuditEventTypeRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new AuditEventTypeEntity(request.Code, request.Name, request.Description, request.Category, request.RiskLevel, request.SortOrder);
        return core.CreateAsync(entity, CatalogMapping.ToAuditEventTypeDto);
    }

    public Task<AuditEventTypeDto?> UpdateAsync(Guid id, UpdateAuditEventTypeRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.Category, request.RiskLevel, request.SortOrder); CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToAuditEventTypeDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
