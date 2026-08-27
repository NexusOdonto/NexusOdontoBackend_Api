using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.ToothStatus;

namespace Application.Contracts.Services.Catalogs
{
    public interface IToothStatusService
    {
        Task<IReadOnlyList<ToothStatusDto>> GetAllAsync();
        Task<ToothStatusDto?> GetByIdAsync(Guid id);
        Task<ToothStatusDto> CreateAsync(CreateToothStatusRequestDto request);
        Task<ToothStatusDto?> UpdateAsync(Guid id, UpdateToothStatusRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
