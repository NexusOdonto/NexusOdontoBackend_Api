using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogs.FindingStatus;

namespace Application.Contracts.Services.Catalogs
{
    public interface IFindingStatusService
    {
        Task<IReadOnlyList<FindingStatusDto>> GetAllAsync();
        Task<FindingStatusDto?> GetByIdAsync(Guid id);
        Task<FindingStatusDto> CreateAsync(CreateFindingStatusRequestDto request);
        Task<FindingStatusDto?> UpdateAsync(Guid id, UpdateFindingStatusRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
