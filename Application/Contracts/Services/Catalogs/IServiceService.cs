using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Contracts.Services.Catalogs
{
    public interface IServiceService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateServiceRequestDto request);
        Task<object?> UpdateAsync(Guid id, CreateServiceRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
