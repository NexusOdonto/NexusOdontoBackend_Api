using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Clinical.ClinicalAttention;

namespace Application.Contracts.Services.Clinical
{
    public interface IClinicalAttentionService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateClinicalAttentionRequestDto request);
        Task<object?> UpdateAsync(Guid id, UpdateClinicalAttentionRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
