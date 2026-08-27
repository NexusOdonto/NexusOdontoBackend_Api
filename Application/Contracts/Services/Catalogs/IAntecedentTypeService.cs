using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.AntecedentType;

namespace Application.Contracts.Services.Catalogs
{
    public interface IAntecedentTypeService
    {
        Task<IReadOnlyList<AntecedentTypeDto>> GetAllAsync();
        Task<AntecedentTypeDto?> GetByIdAsync(Guid id);
        Task<AntecedentTypeDto> CreateAsync(CreateAntecedentTypeRequestDto request);
        Task<AntecedentTypeDto?> UpdateAsync(Guid id, UpdateAntecedentTypeRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
