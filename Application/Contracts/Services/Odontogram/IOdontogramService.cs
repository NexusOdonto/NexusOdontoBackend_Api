using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Odontogram.Odontogram;

namespace Application.Contracts.Services.Odontogram
{
    public interface IOdontogramService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateOdontogramRequestDto request);
        Task<object?> UpdateAsync(Guid id, UpdateOdontogramRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
