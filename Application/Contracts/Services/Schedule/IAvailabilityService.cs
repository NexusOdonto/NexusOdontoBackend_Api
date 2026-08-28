using Application.Contracts.Services.Common;
using Application.DTOs.Availabilities;

namespace Application.Contracts.Services.Schedule
{
    public interface IAvailabilityService : ICrudService<AvailabilityResponseDto, CreateAvailabilityDto, UpdateAvailabilityDto>
    {
    }
}
