using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.DocumentType;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class DocumentTypeService(CatalogServiceCore<DocumentTypeEntity> core) : IDocumentTypeService
{
    public Task<IReadOnlyList<DocumentTypeDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToDocumentTypeDto);

    public Task<DocumentTypeDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToDocumentTypeDto);

    public Task<DocumentTypeDto> CreateAsync(DocumentTypeDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new DocumentTypeEntity(request.Code, request.Name, request.Description, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToDocumentTypeDto);
    }

    public Task<DocumentTypeDto?> UpdateAsync(Guid id, DocumentTypeDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToDocumentTypeDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
