using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Odontogram.DentalFinding;

namespace Application.Contracts.Services.Odontogram
{
    public interface IDentalFindingService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateDentalFindingRequestDto request);
        Task<object?> UpdateAsync(Guid id, UpdateDentalFindingRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
