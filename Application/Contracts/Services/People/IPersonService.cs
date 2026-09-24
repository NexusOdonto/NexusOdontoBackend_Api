using Application.Contracts.Services.Common;
using Application.DTOs.People;

namespace Application.Contracts.Services.People
{
    public interface IPersonService : ICrudService<PersonResponseDto, CreatePersonDto, UpdatePersonDto>
    {
        /// <summary>
        /// Resolves a person by document number (cédula) only — the unique identity key.
        /// </summary>
        Task<PersonResponseDto?> GetByDocumentNumberAsync(
            string documentNumber,
            CancellationToken cancellationToken = default);
    }
}
