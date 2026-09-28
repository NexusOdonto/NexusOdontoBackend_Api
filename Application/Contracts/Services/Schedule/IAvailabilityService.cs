using Application.Contracts.Services.Common;
using Application.DTOs.Appointments;
using Application.DTOs.Availabilities;

namespace Application.Contracts.Services.Schedule
{
    public interface IAvailabilityService : ICrudService<AvailabilityResponseDto, CreateAvailabilityDto, UpdateAvailabilityDto>
    {
        Task<IReadOnlyList<AffectedAppointmentDto>> GetImpactAsync(
            AvailabilityImpactRequestDto request,
            CancellationToken cancellationToken = default);

        Task<ReplaceProfessionalAvailabilityResultDto> ReplaceForProfessionalAsync(
            Guid professionalId,
            ReplaceProfessionalAvailabilityDto request,
            Guid? requestedByUserId = null,
            CancellationToken cancellationToken = default);
    }
}
