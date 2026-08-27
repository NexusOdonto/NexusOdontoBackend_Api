using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Sesiones;

namespace Application.Contracts.Services.People
{
    public interface ISessionService
    {
        Task<IReadOnlyList<SesionDto>> GetAllAsync();
        Task<SesionDto?> GetByIdAsync(Guid id);
        Task<SesionDto> CreateAsync(SesionDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
