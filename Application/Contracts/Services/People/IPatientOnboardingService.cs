using Application.DTOs.Patients;

namespace Application.Contracts.Services.People;

public interface IPatientOnboardingService
{
    Task<OnboardPatientResponseDto> OnboardAsync(
        OnboardPatientDto request,
        CancellationToken cancellationToken = default);
}
