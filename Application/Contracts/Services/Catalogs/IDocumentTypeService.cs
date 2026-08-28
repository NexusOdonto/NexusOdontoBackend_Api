using Application.DTOs.Catalogs.DocumentType;

namespace Application.Contracts.Services.Catalogs
{
    public interface IDocumentTypeService
    {
        Task<IReadOnlyList<DocumentTypeDto>> GetAllAsync();
        Task<DocumentTypeDto?> GetByIdAsync(Guid id);
        Task<DocumentTypeDto> CreateAsync(DocumentTypeDto request);
        Task<DocumentTypeDto?> UpdateAsync(Guid id, DocumentTypeDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
