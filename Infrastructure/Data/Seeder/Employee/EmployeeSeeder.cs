using Domain.Entities.People;
using Infrastructure.Data.Seeder.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Employee;

public static class EmployeeSeeder
{
    public static readonly Guid AdminEmployeeId = new("1c000000-0000-0000-0000-000000000001");
    public static readonly Guid Dentist1EmployeeId = new("1c000000-0000-0000-0000-000000000002");
    public static readonly Guid Dentist2EmployeeId = new("1c000000-0000-0000-0000-000000000003");
    public static readonly Guid Dentist3EmployeeId = new("1c000000-0000-0000-0000-000000000004");
    public static readonly Guid ReceptionistEmployeeId = new("1c000000-0000-0000-0000-000000000005");
    public static readonly Guid BotEmployeeId = new("1c000000-0000-0000-0000-000000000006");

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var hireDate = new DateOnly(2026, 1, 15);

        modelBuilder.Entity<EmployeeEntity>().HasData(
            new
            {
                Id = AdminEmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000001"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000001"),
                EmployeeCode = "AD001",
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Dentist1EmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000003"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000003"),
                EmployeeCode = "OD001",
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Dentist2EmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000010"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000003"),
                EmployeeCode = "OD002",
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Dentist3EmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000011"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000003"),
                EmployeeCode = "OD003",
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = ReceptionistEmployeeId,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000005"),
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000002"),
                EmployeeCode = "RC001",
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = BotEmployeeId,
                PersonId = UserSeeder.BotPersonId,
                JobTitleId = new Guid("11000000-0000-0000-0000-000000000004"),
                EmployeeCode = "BOT001",
                HireDate = hireDate,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
