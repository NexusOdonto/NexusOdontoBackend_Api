using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.FindingType;

namespace Application.Contracts.Services.Catalogs
{
    public interface IFindingTypeService
    {
        Task<IReadOnlyList<FindingTypeDto>> GetAllAsync();
        Task<FindingTypeDto?> GetByIdAsync(Guid id);
        Task<FindingTypeDto> CreateAsync(CreateFindingTypeRequestDto request);
        Task<FindingTypeDto?> UpdateAsync(Guid id, UpdateFindingTypeRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
