using Application.DTOs.Catalogs.AppointmentStatus;

namespace Application.Contracts.Services.Catalogs
{
    public interface IAppointmentStatusService
    {
        Task<IReadOnlyList<AppointmentStatusDto>> GetAllAsync();
        Task<AppointmentStatusDto?> GetByIdAsync(Guid id);
        Task<AppointmentStatusDto> CreateAsync(AppointmentStatusDto request);
        Task<AppointmentStatusDto?> UpdateAsync(Guid id, AppointmentStatusDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
