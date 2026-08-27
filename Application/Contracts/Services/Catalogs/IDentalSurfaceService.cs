using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.DentalSurface;

namespace Application.Contracts.Services.Catalogs
{
    public interface IDentalSurfaceService
    {
        Task<IReadOnlyList<DentalSurfaceDto>> GetAllAsync();
        Task<DentalSurfaceDto?> GetByIdAsync(Guid id);
        Task<DentalSurfaceDto> CreateAsync(CreateDentalSurfaceRequestDto request);
        Task<DentalSurfaceDto?> UpdateAsync(Guid id, UpdateDentalSurfaceRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
