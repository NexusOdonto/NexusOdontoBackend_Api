using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Catalogos;

namespace Application.Contracts.Services.Catalogs
{
    public interface IDocumentTypeService
    {
        Task<IReadOnlyList<TipoDocumentoDto>> GetAllAsync();
        Task<TipoDocumentoDto?> GetByIdAsync(Guid id);
        Task<TipoDocumentoDto> CreateAsync(TipoDocumentoDto request);
        Task<TipoDocumentoDto?> UpdateAsync(Guid id, TipoDocumentoDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
