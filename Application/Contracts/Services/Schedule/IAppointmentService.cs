using Application.Contracts.Services.Common;
using Application.DTOs.Appointments;

namespace Application.Contracts.Services.Schedule
{
    public interface IAppointmentService : ICrudService<AppointmentResponseDto, CreateAppointmentDto, UpdateAppointmentDto>
    {
        Task<AppointmentClinicalContextDto?> GetClinicalContextAsync(Guid appointmentId, CancellationToken cancellationToken = default);
        Task<AppointmentResponseDto?> MarkAsNoShowAsync(Guid appointmentId, string? reason = null, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RequiresRescheduleAppointmentDto>> GetRequiresRescheduleAsync(CancellationToken cancellationToken = default);
    }
}
