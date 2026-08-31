using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Employee;

public static class EmployeeSeeder
{
    public static readonly Guid AdminEmployeeId = new("1c000000-0000-0000-0000-000000000001");
    public static readonly Guid Dentist1EmployeeId = new("1c000000-0000-0000-0000-000000000002");
    public static readonly Guid Dentist2EmployeeId = new("1c000000-0000-0000-0000-000000000003");
    public static readonly Guid Dentist3EmployeeId = new("1c000000-0000-0000-0000-000000000004");
    public static readonly Guid ReceptionistEmployeeId = new("1c000000-0000-0000-0000-000000000005");
    public static readonly Guid AssistantEmployeeId = new("1c000000-0000-0000-0000-000000000006");

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var hireDate = new DateOnly(2026, 1, 15);

        modelBuilder.Entity<EmployeeEntity>().HasData(
            // 1. Carlos Administrator
            new
            {
                Id = AdminEmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000001"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000001"), // ADMINISTRADOR
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 2. Dra. Laura Gómez (Dentist 1)
            new
            {
                Id = Dentist1EmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000003"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000003"), // ODONTOLOGO
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 3. Dr. Roberto Martínez (Dentist 2)
            new
            {
                Id = Dentist2EmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000010"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000003"), // ODONTOLOGO
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 4. Dra. Ana Sofía Silva (Dentist 3)
            new
            {
                Id = Dentist3EmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000011"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000003"), // ODONTOLOGO
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 5. María Rodríguez (Receptionist)
            new
            {
                Id = ReceptionistEmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000005"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000002"), // RECEPCIONISTA
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 6. Juan Castro (Assistant)
            new
            {
                Id = AssistantEmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000012"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000004"), // ASISTENTE
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
