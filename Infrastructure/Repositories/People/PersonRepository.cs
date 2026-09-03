using Application.Contracts.Repositories.People;
using Domain.Entities.People;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.People;

public sealed class PersonRepository : Common.Repository<PersonEntity>, IPersonRepository
{
    public PersonRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PersonEntity?> GetByDocumentAsync(
        Guid documentTypeId,
        string documentNumber,
        CancellationToken cancellationToken = default)
    {
        _ = documentTypeId;
        return await GetByDocumentNumberAsync(documentNumber, cancellationToken);
    }

    public async Task<PersonEntity?> GetByDocumentNumberAsync(
        string documentNumber,
        CancellationToken cancellationToken = default)
    {
        var normalizedDocument = documentNumber.Trim();
        return await Set.SingleOrDefaultAsync(
            p => p.DocumentNumber == normalizedDocument,
            cancellationToken);
    }

    public async Task<PersonEntity?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return await Set.SingleOrDefaultAsync(
            p => p.Email == normalizedEmail,
            cancellationToken);
    }
}
