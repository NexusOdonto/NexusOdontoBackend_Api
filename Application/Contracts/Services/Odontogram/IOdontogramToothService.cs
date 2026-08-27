using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Odontogram.OdontogramTooth;

namespace Application.Contracts.Services.Odontogram
{
    public interface IOdontogramToothService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateOdontogramToothRequestDto request);
        Task<object?> UpdateAsync(Guid id, UpdateOdontogramToothRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
