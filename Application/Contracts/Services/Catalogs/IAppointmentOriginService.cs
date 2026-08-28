using Application.DTOs.Catalogs.AppointmentOrigin;

namespace Application.Contracts.Services.Catalogs
{
    public interface IAppointmentOriginService
    {
        Task<IReadOnlyList<AppointmentOriginDto>> GetAllAsync();
        Task<AppointmentOriginDto?> GetByIdAsync(Guid id);
        Task<AppointmentOriginDto> CreateAsync(AppointmentOriginDto request);
        Task<AppointmentOriginDto?> UpdateAsync(Guid id, AppointmentOriginDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
