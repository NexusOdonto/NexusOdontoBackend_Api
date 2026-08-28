using Application.Contracts.Repositories.Schedule;
using Domain.Entities.Schedule;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Schedule;

public sealed class AppointmentRepository : Common.Repository<AppointmentEntity>, IAppointmentRepository
{
    public AppointmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<AppointmentEntity>> GetByProfessionalInRangeAsync(
        Guid professionalId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Where(a =>
                a.ProfessionalId == professionalId &&
                a.StartsAt < to &&
                a.EndsAt > from)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AppointmentEntity>> GetByPatientAsync(
        Guid patientId,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Where(a => a.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }
}
