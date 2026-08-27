using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Clinical.ProcedurePerformed;

namespace Application.Contracts.Services.Clinical
{
    public interface IProcedurePerformedService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(CreateProcedurePerformedRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
