using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class AppointmentStatusService(CatalogServiceCore<AppointmentStatusEntity> core) : IAppointmentStatusService
{
    public Task<IReadOnlyList<EstadoCitaDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToEstadoCitaDto);

    public Task<EstadoCitaDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToEstadoCitaDto);

    public Task<EstadoCitaDto> CreateAsync(EstadoCitaDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new AppointmentStatusEntity(request.Codigo, request.Nombre, request.Descripcion, request.ColorHex, request.Orden, request.EsFinal);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToEstadoCitaDto);
    }

    public Task<EstadoCitaDto?> UpdateAsync(Guid id, EstadoCitaDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.ColorHex, request.Orden, request.EsFinal); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToEstadoCitaDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
