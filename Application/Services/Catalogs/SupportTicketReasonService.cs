using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.SupportTicketReason;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class SupportTicketReasonService(CatalogServiceCore<SupportTicketReasonEntity> core) : ISupportTicketReasonService
{
    public Task<IReadOnlyList<SupportTicketReasonDto>> GetAllAsync() =>
        core.GetAllAsync(EnglishCatalogMapping.ToSupportTicketReasonDto);

    public Task<SupportTicketReasonDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, EnglishCatalogMapping.ToSupportTicketReasonDto);

    public Task<SupportTicketReasonDto> CreateAsync(CreateSupportTicketReasonRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new SupportTicketReasonEntity(request.Code, request.Name, request.Description, request.SortOrder);
        return core.CreateAsync(entity, EnglishCatalogMapping.ToSupportTicketReasonDto);
    }

    public Task<SupportTicketReasonDto?> UpdateAsync(Guid id, UpdateSupportTicketReasonRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder); EnglishCatalogMapping.ApplyActive(entity, request.IsActive);
        }, EnglishCatalogMapping.ToSupportTicketReasonDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
