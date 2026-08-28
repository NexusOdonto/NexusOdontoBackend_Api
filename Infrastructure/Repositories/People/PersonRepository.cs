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
        return await Set.SingleOrDefaultAsync(
            p => p.DocumentTypeId == documentTypeId && p.DocumentNumber == documentNumber,
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
