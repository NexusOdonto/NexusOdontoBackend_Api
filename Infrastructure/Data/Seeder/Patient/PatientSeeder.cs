using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Patient;

public static class PatientSeeder
{
    public static readonly Guid Patient1Id = new("1e000000-0000-0000-0000-000000000001");
    public static readonly Guid Patient2Id = new("1e000000-0000-0000-0000-000000000002");
    public static readonly Guid Patient3Id = new("1e000000-0000-0000-0000-000000000003");
    public static readonly Guid Patient4Id = new("1e000000-0000-0000-0000-000000000004");

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<PatientEntity>().HasData(
            // 1. Andrés Pérez
            new
            {
                Id = Patient1Id,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000007"),
                EmergencyContact = "Lucía Pérez (Madre)",
                EmergencyPhone = "+573101234567",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 2. Camila López
            new
            {
                Id = Patient2Id,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000013"),
                EmergencyContact = "Jorge López (Padre)",
                EmergencyPhone = "+573112345678",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 3. Felipe Torres
            new
            {
                Id = Patient3Id,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000014"),
                EmergencyContact = "Marta Torres (Esposa)",
                EmergencyPhone = "+573123456789",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 4. Valentina Mendoza
            new
            {
                Id = Patient4Id,
                PersonId = new Guid("1b000000-0000-0000-0000-000000000015"),
                EmergencyContact = "Carlos Mendoza (Hermano)",
                EmergencyPhone = "+573134567890",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
