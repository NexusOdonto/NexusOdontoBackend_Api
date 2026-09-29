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

        /// <summary>Rows of the professional that apply on the given date (Colombia today when null).</summary>
        Task<IReadOnlyList<AvailabilityResponseDto>> GetEffectiveForProfessionalAsync(
            Guid professionalId,
            DateOnly? date,
            CancellationToken cancellationToken = default);

        Task<ReplaceProfessionalAvailabilityResultDto> ReplaceForProfessionalAsync(
            Guid professionalId,
            ReplaceProfessionalAvailabilityDto request,
            Guid? requestedByUserId = null,
            CancellationToken cancellationToken = default);
    }
}
