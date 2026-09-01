using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedProfessionalSpecialties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "professional_specialties",
                columns: ["professional_id", "specialty_id"],
                values: new object[,]
                {
                    { new Guid("1d000000-0000-0000-0000-000000000001"), new Guid("18000000-0000-0000-0000-000000000001") },
                    { new Guid("1d000000-0000-0000-0000-000000000001"), new Guid("18000000-0000-0000-0000-000000000002") },
                    { new Guid("1d000000-0000-0000-0000-000000000002"), new Guid("18000000-0000-0000-0000-000000000003") },
                    { new Guid("1d000000-0000-0000-0000-000000000002"), new Guid("18000000-0000-0000-0000-000000000006") },
                    { new Guid("1d000000-0000-0000-0000-000000000003"), new Guid("18000000-0000-0000-0000-000000000004") },
                    { new Guid("1d000000-0000-0000-0000-000000000003"), new Guid("18000000-0000-0000-0000-000000000005") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "professional_specialties",
                keyColumns: ["professional_id", "specialty_id"],
                keyValues: [new Guid("1d000000-0000-0000-0000-000000000001"), new Guid("18000000-0000-0000-0000-000000000001")]);

            migrationBuilder.DeleteData(
                table: "professional_specialties",
                keyColumns: ["professional_id", "specialty_id"],
                keyValues: [new Guid("1d000000-0000-0000-0000-000000000001"), new Guid("18000000-0000-0000-0000-000000000002")]);

            migrationBuilder.DeleteData(
                table: "professional_specialties",
                keyColumns: ["professional_id", "specialty_id"],
                keyValues: [new Guid("1d000000-0000-0000-0000-000000000002"), new Guid("18000000-0000-0000-0000-000000000003")]);

            migrationBuilder.DeleteData(
                table: "professional_specialties",
                keyColumns: ["professional_id", "specialty_id"],
                keyValues: [new Guid("1d000000-0000-0000-0000-000000000002"), new Guid("18000000-0000-0000-0000-000000000006")]);

            migrationBuilder.DeleteData(
                table: "professional_specialties",
                keyColumns: ["professional_id", "specialty_id"],
                keyValues: [new Guid("1d000000-0000-0000-0000-000000000003"), new Guid("18000000-0000-0000-0000-000000000004")]);

            migrationBuilder.DeleteData(
                table: "professional_specialties",
                keyColumns: ["professional_id", "specialty_id"],
                keyValues: [new Guid("1d000000-0000-0000-0000-000000000003"), new Guid("18000000-0000-0000-0000-000000000005")]);
        }
    }
}
