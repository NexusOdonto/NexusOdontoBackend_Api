using Application.Contracts.Repositories.Common;
using Domain.Entities.Schedule;

namespace Application.Contracts.Repositories.Schedule;

public interface IAppointmentRepository : IRepository<AppointmentEntity>
{
    Task<IReadOnlyList<AppointmentEntity>> GetByProfessionalInRangeAsync(
        Guid professionalId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AppointmentEntity>> GetByPatientAsync(
        Guid patientId,
        CancellationToken cancellationToken = default);
}
