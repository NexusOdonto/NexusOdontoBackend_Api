using Application.Contracts.Services.Common;
using Application.DTOs.Clinical.PatientAntecedent;

namespace Application.Contracts.Services.Clinical
{
    public interface IPatientAntecedentService : ICrudService<PatientAntecedentResponseDto, CreatePatientAntecedentRequestDto, UpdatePatientAntecedentRequestDto>
    {
    }
}
