using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface IJobTitleService
    {
        Task<IReadOnlyList<CargoDto>> GetAllAsync();
        Task<CargoDto?> GetByIdAsync(Guid id);
        Task<CargoDto> CreateAsync(CargoDto request);
        Task<CargoDto?> UpdateAsync(Guid id, CargoDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
