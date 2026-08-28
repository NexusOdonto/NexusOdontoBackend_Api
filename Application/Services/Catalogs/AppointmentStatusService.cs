using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.AppointmentStatus;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class AppointmentStatusService(CatalogServiceCore<AppointmentStatusEntity> core) : IAppointmentStatusService
{
    public Task<IReadOnlyList<AppointmentStatusDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToAppointmentStatusDto);

    public Task<AppointmentStatusDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToAppointmentStatusDto);

    public Task<AppointmentStatusDto> CreateAsync(AppointmentStatusDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new AppointmentStatusEntity(request.Code, request.Name, request.Description, request.ColorHex, request.SortOrder, request.IsFinal);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToAppointmentStatusDto);
    }

    public Task<AppointmentStatusDto?> UpdateAsync(Guid id, AppointmentStatusDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.ColorHex, request.SortOrder, request.IsFinal);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToAppointmentStatusDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
