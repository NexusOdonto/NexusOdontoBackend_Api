using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class DocumentTypeService(CatalogServiceCore<DocumentType> core) : IDocumentTypeService
{
    public Task<IReadOnlyList<TipoDocumentoDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToTipoDocumentoDto);

    public Task<TipoDocumentoDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToTipoDocumentoDto);

    public Task<TipoDocumentoDto> CreateAsync(TipoDocumentoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new DocumentType(request.Codigo, request.Nombre, request.Descripcion, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToTipoDocumentoDto);
    }

    public Task<TipoDocumentoDto?> UpdateAsync(Guid id, TipoDocumentoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToTipoDocumentoDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
