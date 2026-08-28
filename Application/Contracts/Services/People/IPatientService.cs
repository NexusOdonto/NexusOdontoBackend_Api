using Application.Contracts.Services.Common;
using Application.DTOs.Patients;

namespace Application.Contracts.Services.People
{
    public interface IPatientService : ICrudService<PatientResponseDto, CreatePatientDto, UpdatePatientDto>
    {
    }
}
