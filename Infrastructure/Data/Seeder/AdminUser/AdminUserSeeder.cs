using Domain.Entities.People;
using Domain.Entities.Security;
using Infrastructure.Data.Seeder.Role;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.AdminUser;

public static class AdminUserSeeder
{
    public static readonly Guid AdminPersonId = new("1b000000-0000-0000-0000-000000000001");
    public static readonly Guid AdminUserId = new("1b000000-0000-0000-0000-000000000002");

    public static readonly Guid DentistPersonId = new("1b000000-0000-0000-0000-000000000003");
    public static readonly Guid DentistUserId = new("1b000000-0000-0000-0000-000000000004");

    public static readonly Guid ReceptionistPersonId = new("1b000000-0000-0000-0000-000000000005");
    public static readonly Guid ReceptionistUserId = new("1b000000-0000-0000-0000-000000000006");

    public static readonly Guid PatientPersonId = new("1b000000-0000-0000-0000-000000000007");
    public static readonly Guid PatientUserId = new("1b000000-0000-0000-0000-000000000008");

    // BCrypt hashes (work factor 11)
    private const string AdminPasswordHash = "$2a$11$niKp.bG.Nj.pEEV1ZZY41.MEByC9NN514GjlTZfPuYt.EP/PEO9QG"; // Admin123!
    private const string DentistPasswordHash = "$2a$11$XIu0y.lHkWLbOkKZHjlxYerEkr/qEHhqR2qhIY8Q3FkXYbm1pOSN6"; // Doctor123!
    private const string ReceptionistPasswordHash = "$2a$11$VdUVA/jwbm0gDxIOfNs0/OEzCQ.ea2DB48nz7LFJOv1xdkSkhhS9."; // Recepcion123!
    private const string PatientPasswordHash = "$2a$11$wBnMjN.GsYVvbI4loTatCO8H12Ljz9RZbCzbayXngNgRU1WwvLJ1m"; // Paciente123!

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var documentTypeId = new Guid("e0000000-0000-0000-0000-000000000001"); // CC

        modelBuilder.Entity<PersonEntity>().HasData(
            // 1. Carlos Administrator
            new
            {
                Id = AdminPersonId,
                DocumentTypeId = documentTypeId,
                DocumentNumber = "1234567890",
                FirstName = "Carlos",
                LastName = "Administrator",
                DateOfBirth = (DateOnly?)null,
                SexId = (Guid?)null,
                Phone = (string?)null,
                Email = "admin@nexusodonto.com",
                Address = (string?)null,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 2. Laura Gómez (Odontólogo)
            new
            {
                Id = DentistPersonId,
                DocumentTypeId = documentTypeId,
                DocumentNumber = "1098765432",
                FirstName = "Laura",
                LastName = "Gómez",
                DateOfBirth = (DateOnly?)null,
                SexId = (Guid?)null,
                Phone = (string?)null,
                Email = "odontologo@nexusodonto.com",
                Address = (string?)null,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 3. María Rodríguez (Recepcionista)
            new
            {
                Id = ReceptionistPersonId,
                DocumentTypeId = documentTypeId,
                DocumentNumber = "1087654321",
                FirstName = "María",
                LastName = "Rodríguez",
                DateOfBirth = (DateOnly?)null,
                SexId = (Guid?)null,
                Phone = (string?)null,
                Email = "recepcion@nexusodonto.com",
                Address = (string?)null,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 4. Andrés Pérez (Paciente)
            new
            {
                Id = PatientPersonId,
                DocumentTypeId = documentTypeId,
                DocumentNumber = "1076543210",
                FirstName = "Andrés",
                LastName = "Pérez",
                DateOfBirth = (DateOnly?)null,
                SexId = (Guid?)null,
                Phone = (string?)null,
                Email = "paciente@nexusodonto.com",
                Address = (string?)null,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });

        modelBuilder.Entity<UserEntity>().HasData(
            new
            {
                Id = AdminUserId,
                PersonId = AdminPersonId,
                PasswordHash = AdminPasswordHash,
                LastAccessAt = (DateTime?)null,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = DentistUserId,
                PersonId = DentistPersonId,
                PasswordHash = DentistPasswordHash,
                LastAccessAt = (DateTime?)null,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = ReceptionistUserId,
                PersonId = ReceptionistPersonId,
                PasswordHash = ReceptionistPasswordHash,
                LastAccessAt = (DateTime?)null,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = PatientUserId,
                PersonId = PatientPersonId,
                PasswordHash = PatientPasswordHash,
                LastAccessAt = (DateTime?)null,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });

        modelBuilder.Entity<UserRoleEntity>().HasData(
            new
            {
                UserId = AdminUserId,
                RoleId = RoleSeeder.Administrator,
                AssignedAt = seedCreatedAt
            },
            new
            {
                UserId = DentistUserId,
                RoleId = RoleSeeder.Dentist,
                AssignedAt = seedCreatedAt
            },
            new
            {
                UserId = ReceptionistUserId,
                RoleId = RoleSeeder.Receptionist,
                AssignedAt = seedCreatedAt
            },
            new
            {
                UserId = PatientUserId,
                RoleId = RoleSeeder.Patient,
                AssignedAt = seedCreatedAt
            });
    }
}
