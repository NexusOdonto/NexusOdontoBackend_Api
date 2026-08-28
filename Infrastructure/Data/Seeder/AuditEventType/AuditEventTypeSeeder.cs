using AuditEventTypeEntity = Domain.Entities.Catalogs.AuditEventTypeEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.AuditEventType;

public static class AuditEventTypeSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<AuditEventTypeEntity>().HasData(
            new
            {
                Id = new Guid("16000000-0000-0000-0000-000000000001"),
                Code = "INICIO_SESION",
                Name = "Sign In",
                Description = "A user signed in to the system.",
                Category = "SEGURIDAD",
                RiskLevel = (int?)2,
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("16000000-0000-0000-0000-000000000002"),
                Code = "AGENDAR_CITA",
                Name = "Schedule Appointment",
                Description = "An appointment was created for a patient.",
                Category = "AGENDA",
                RiskLevel = (int?)1,
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("16000000-0000-0000-0000-000000000003"),
                Code = "CERRAR_TICKET",
                Name = "Close Ticket",
                Description = "A support ticket was closed.",
                Category = "SOPORTE",
                RiskLevel = (int?)1,
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("16000000-0000-0000-0000-000000000004"),
                Code = "MODIFICAR_HISTORIA",
                Name = "Modify Clinical History",
                Description = "A clinical history record was modified.",
                Category = "CLINICO",
                RiskLevel = (int?)3,
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
