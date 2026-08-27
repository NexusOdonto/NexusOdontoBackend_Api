using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts.Services.Audit
{
    public interface IAuditEventService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(object request);
        Task<bool> DeleteAsync(Guid id);
    }
}
