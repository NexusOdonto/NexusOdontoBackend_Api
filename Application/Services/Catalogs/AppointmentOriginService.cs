using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class AppointmentOriginService(CatalogServiceCore<AppointmentOrigin> core) : IAppointmentOriginService
{
    public Task<IReadOnlyList<OrigenCitaDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToOrigenCitaDto);

    public Task<OrigenCitaDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToOrigenCitaDto);

    public Task<OrigenCitaDto> CreateAsync(OrigenCitaDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new AppointmentOrigin(request.Codigo, request.Nombre, request.Descripcion, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToOrigenCitaDto);
    }

    public Task<OrigenCitaDto?> UpdateAsync(Guid id, OrigenCitaDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToOrigenCitaDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
