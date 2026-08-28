using Application.Contracts.Services.Common;
using Application.DTOs.Appointments;

namespace Application.Contracts.Services.Schedule
{
    public interface IAppointmentService : ICrudService<AppointmentResponseDto, CreateAppointmentDto, UpdateAppointmentDto>
    {
    }
}
