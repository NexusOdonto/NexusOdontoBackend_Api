using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Clinical.PatientAntecedent;

namespace Application.Contracts.Services.Clinical
{
    public interface IPatientAntecedentService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreatePatientAntecedentRequestDto request);
        Task<object?> UpdateAsync(Guid id, UpdatePatientAntecedentRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
