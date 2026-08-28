using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.AppointmentOrigin;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class AppointmentOriginService(CatalogServiceCore<AppointmentOriginEntity> core) : IAppointmentOriginService
{
    public Task<IReadOnlyList<AppointmentOriginDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToAppointmentOriginDto);

    public Task<AppointmentOriginDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToAppointmentOriginDto);

    public Task<AppointmentOriginDto> CreateAsync(AppointmentOriginDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new AppointmentOriginEntity(request.Code, request.Name, request.Description, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToAppointmentOriginDto);
    }

    public Task<AppointmentOriginDto?> UpdateAsync(Guid id, AppointmentOriginDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToAppointmentOriginDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
