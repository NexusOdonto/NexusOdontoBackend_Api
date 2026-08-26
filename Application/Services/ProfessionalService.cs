using Application.Contracts.Repositories;
using Application.Exceptions;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Application.Services;

public sealed class ProfessionalService(
    IRepository<Employee> employeeRepository,
    IRepository<Professional> professionalRepository,
    IRepository<Specialty> specialtyRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Guid> CreateAsync(
        Guid personId,
        Guid jobTitleId,
        DateTime hireDate,
        string professionalLicense,
        IEnumerable<Guid> specialtyIds,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(specialtyIds);

        await unitOfWork.BeginTransactionAsync();

        try
        {
            var employee = new Employee(personId, jobTitleId, hireDate);
            await employeeRepository.AddAsync(employee);

            var professional = new Professional(employee.Id, professionalLicense);

            foreach (var specialtyId in specialtyIds.Distinct())
            {
                var specialty = await specialtyRepository.GetByIdAsync(specialtyId)
                    ?? throw new NotFoundException($"Specialty {specialtyId} was not found.");
                professional.Specialties.Add(specialty);
            }

            await professionalRepository.AddAsync(professional);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync();

            return professional.Id;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
