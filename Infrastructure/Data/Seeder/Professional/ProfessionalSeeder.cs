using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Professional;

public static class ProfessionalSeeder
{
    public static readonly Guid Dentist1ProfessionalId = new("1d000000-0000-0000-0000-000000000001");
    public static readonly Guid Dentist2ProfessionalId = new("1d000000-0000-0000-0000-000000000002");
    public static readonly Guid Dentist3ProfessionalId = new("1d000000-0000-0000-0000-000000000003");

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<ProfessionalEntity>().HasData(
            // 1. Dra. Laura Gómez
            new
            {
                Id = Dentist1ProfessionalId,
                EmployeeId = new Guid("1c000000-0000-0000-0000-000000000002"),
                ProfessionalLicense = "TP-OD-1098765",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 2. Dr. Roberto Martínez
            new
            {
                Id = Dentist2ProfessionalId,
                EmployeeId = new Guid("1c000000-0000-0000-0000-000000000003"),
                ProfessionalLicense = "TP-OD-1098766",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            // 3. Dra. Ana Sofía Silva
            new
            {
                Id = Dentist3ProfessionalId,
                EmployeeId = new Guid("1c000000-0000-0000-0000-000000000004"),
                ProfessionalLicense = "TP-OD-1098767",
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });

        // Seed professional_specialties join table
        modelBuilder.Entity("professional_specialties").HasData(
            // Dra. Laura Gómez -> General Dentistry & Orthodontics
            new
            {
                professional_id = Dentist1ProfessionalId,
                specialty_id = new Guid("18000000-0000-0000-0000-000000000001") // ODONTOLOGIA_GENERAL
            },
            new
            {
                professional_id = Dentist1ProfessionalId,
                specialty_id = new Guid("18000000-0000-0000-0000-000000000002") // ORTODONCIA
            },
            // Dr. Roberto Martínez -> Endodontics & Oral Surgery
            new
            {
                professional_id = Dentist2ProfessionalId,
                specialty_id = new Guid("18000000-0000-0000-0000-000000000003") // ENDODONCIA
            },
            new
            {
                professional_id = Dentist2ProfessionalId,
                specialty_id = new Guid("18000000-0000-0000-0000-000000000006") // CIRUGIA_ORAL
            },
            // Dra. Ana Sofía Silva -> Periodontics & Pediatric Dentistry
            new
            {
                professional_id = Dentist3ProfessionalId,
                specialty_id = new Guid("18000000-0000-0000-0000-000000000004") // PERIODONCIA
            },
            new
            {
                professional_id = Dentist3ProfessionalId,
                specialty_id = new Guid("18000000-0000-0000-0000-000000000005") // ODONTOPEDIATRIA
            });
    }
}
