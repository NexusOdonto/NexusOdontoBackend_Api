using Application.DTOs.Patients;

namespace Application.DTOs.Auth;

public sealed class GoogleRegisterRequestDto : OnboardPatientDto
{
    public string GoogleSub { get; set; } = string.Empty;
}
