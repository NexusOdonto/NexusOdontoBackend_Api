using Application.DTOs.Patients;

namespace Application.Contracts.Services.People
{
    public interface IPatientService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreatePatientDto request);
        Task<object?> UpdateAsync(Guid id, UpdatePatientDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
