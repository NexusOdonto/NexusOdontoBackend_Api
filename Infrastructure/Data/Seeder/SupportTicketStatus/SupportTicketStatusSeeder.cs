using SupportTicketStatusEntity = Domain.Entities.Catalogs.SupportTicketStatusEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.SupportTicketStatus;

public static class SupportTicketStatusSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<SupportTicketStatusEntity>().HasData(
            new
            {
                Id = new Guid("c0000000-0000-0000-0000-000000000001"),
                Code = "ABIERTO",
                Name = "Open",
                Description = "The support ticket is open and available for assignment.",
                AllowsAssignment = true,
                IsFinal = false,
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("c0000000-0000-0000-0000-000000000002"),
                Code = "ASIGNADO",
                Name = "Assigned",
                Description = "The support ticket has been assigned to a human agent.",
                AllowsAssignment = false,
                IsFinal = false,
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("c0000000-0000-0000-0000-000000000003"),
                Code = "EN_ATENCION",
                Name = "In Progress",
                Description = "A human agent is actively working on the support ticket.",
                AllowsAssignment = false,
                IsFinal = false,
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("c0000000-0000-0000-0000-000000000004"),
                Code = "RESUELTO",
                Name = "Resolved",
                Description = "The reported support request has been resolved.",
                AllowsAssignment = false,
                IsFinal = true,
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("c0000000-0000-0000-0000-000000000005"),
                Code = "CERRADO",
                Name = "Closed",
                Description = "The support ticket has been closed and requires no further action.",
                AllowsAssignment = false,
                IsFinal = true,
                IsActive = true,
                SortOrder = (int?)5,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

