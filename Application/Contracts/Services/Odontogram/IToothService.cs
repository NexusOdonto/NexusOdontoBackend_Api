using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts.Services.Odontogram
{
    public interface IToothService
    {
        Task<IReadOnlyList<object>> GetAllAsync();
        Task<object?> GetByIdAsync(Guid id);
        Task<object> CreateAsync(object request);
        Task<object?> UpdateAsync(Guid id, object request);
        Task<bool> DeleteAsync(Guid id);
    }
}
