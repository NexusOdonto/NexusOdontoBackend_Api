using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.DentitionType;

namespace Application.Contracts.Services.Catalogs
{
    public interface IDentitionTypeService
    {
        Task<IReadOnlyList<DentitionTypeDto>> GetAllAsync();
        Task<DentitionTypeDto?> GetByIdAsync(Guid id);
        Task<DentitionTypeDto> CreateAsync(CreateDentitionTypeRequestDto request);
        Task<DentitionTypeDto?> UpdateAsync(Guid id, UpdateDentitionTypeRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
