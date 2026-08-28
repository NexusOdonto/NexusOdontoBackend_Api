using Application.DTOs.Appointments;

namespace Application.Contracts.Services.Schedule
{
    public interface IAppointmentService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateAppointmentDto request);
        Task<object?> UpdateAsync(Guid id, UpdateAppointmentDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
