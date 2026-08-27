using SupportTicketReasonEntity = Domain.Entities.Catalogs.SupportTicketReason;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.SupportTicketReason;

public static class SupportTicketReasonSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<SupportTicketReasonEntity>().HasData(
            new
            {
                Id = new Guid("d0000000-0000-0000-0000-000000000001"),
                Code = "SOLICITUD_USUARIO",
                Name = "User Request",
                Description = "The user explicitly requested assistance from a human support agent.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("d0000000-0000-0000-0000-000000000002"),
                Code = "BAJA_CONFIANZA_RAG",
                Name = "Low RAG Confidence",
                Description = "The retrieval system could not produce an answer with sufficient confidence.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("d0000000-0000-0000-0000-000000000003"),
                Code = "ERROR_AGENDAMIENTO",
                Name = "Scheduling Error",
                Description = "The automated assistant could not complete an appointment operation.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("d0000000-0000-0000-0000-000000000004"),
                Code = "RECLAMO_SERVICIO",
                Name = "Service Complaint",
                Description = "The user submitted a complaint about a dental service or received care.",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("d0000000-0000-0000-0000-000000000005"),
                Code = "CONSULTA_COMPLEJA",
                Name = "Complex Inquiry",
                Description = "The inquiry requires professional judgment or information beyond the chatbot scope.",
                IsActive = true,
                SortOrder = (int?)5,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

