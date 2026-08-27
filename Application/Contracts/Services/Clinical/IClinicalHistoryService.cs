using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Clinical.ClinicalHistory;

namespace Application.Contracts.Services.Clinical
{
    public interface IClinicalHistoryService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object?> UpdateAsync(Guid id, UpdateClinicalHistoryRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
