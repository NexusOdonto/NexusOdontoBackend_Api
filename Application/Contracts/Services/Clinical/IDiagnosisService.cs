using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Clinical.Diagnosis;

namespace Application.Contracts.Services.Clinical
{
    public interface IDiagnosisService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateDiagnosisRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
