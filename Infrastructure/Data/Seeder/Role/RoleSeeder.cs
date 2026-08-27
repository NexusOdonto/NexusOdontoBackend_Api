using RoleEntity = Domain.Entities.Security.Role;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Role;

public static class RoleSeeder
{
    public static readonly Guid Administrator = new("13000000-0000-0000-0000-000000000001");
    public static readonly Guid Dentist = new("13000000-0000-0000-0000-000000000002");
    public static readonly Guid Receptionist = new("13000000-0000-0000-0000-000000000003");
    public static readonly Guid Assistant = new("13000000-0000-0000-0000-000000000004");
    public static readonly Guid Patient = new("13000000-0000-0000-0000-000000000005");

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<RoleEntity>().HasData(
            new
            {
                Id = Administrator,
                Code = "ADMINISTRADOR",
                Name = "Administrator",
                Description = "Full access to every module of the system.",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Dentist,
                Code = "ODONTOLOGO",
                Name = "Dentist",
                Description = "Access to clinical records, odontogram and appointments.",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Receptionist,
                Code = "RECEPCIONISTA",
                Name = "Receptionist",
                Description = "Access to patient registration and appointment scheduling.",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Assistant,
                Code = "ASISTENTE",
                Name = "Dental Assistant",
                Description = "Read access to clinical information for assisting procedures.",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Patient,
                Code = "PACIENTE",
                Name = "Patient",
                Description = "Access limited to the patient's own information.",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
