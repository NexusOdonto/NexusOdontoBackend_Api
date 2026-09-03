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
            // Idempotent Oracle MERGE: safe when DatabaseInitializer already linked specialties to professionals.
            migrationBuilder.Sql($"""
                MERGE INTO "professional_specialties" t
                USING (
                  SELECT {Raw(new Guid("1d000000-0000-0000-0000-000000000001"))} AS "professional_id", {Raw(new Guid("18000000-0000-0000-0000-000000000001"))} AS "specialty_id" FROM dual UNION ALL
                  SELECT {Raw(new Guid("1d000000-0000-0000-0000-000000000001"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000002"))} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1d000000-0000-0000-0000-000000000002"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000003"))} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1d000000-0000-0000-0000-000000000002"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000006"))} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1d000000-0000-0000-0000-000000000003"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000004"))} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1d000000-0000-0000-0000-000000000003"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000005"))} FROM dual
                ) s
                ON (t."professional_id" = s."professional_id" AND t."specialty_id" = s."specialty_id")
                WHEN NOT MATCHED THEN
                  INSERT ("professional_id", "specialty_id")
                  VALUES (s."professional_id", s."specialty_id");
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                DELETE FROM "professional_specialties"
                WHERE ("professional_id", "specialty_id") IN (
                  ({Raw(new Guid("1d000000-0000-0000-0000-000000000001"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000001"))}),
                  ({Raw(new Guid("1d000000-0000-0000-0000-000000000001"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000002"))}),
                  ({Raw(new Guid("1d000000-0000-0000-0000-000000000002"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000003"))}),
                  ({Raw(new Guid("1d000000-0000-0000-0000-000000000002"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000006"))}),
                  ({Raw(new Guid("1d000000-0000-0000-0000-000000000003"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000004"))}),
                  ({Raw(new Guid("1d000000-0000-0000-0000-000000000003"))}, {Raw(new Guid("18000000-0000-0000-0000-000000000005"))})
                );
                """);
        }

        private static string Raw(Guid id) =>
            $"HEXTORAW('{BitConverter.ToString(id.ToByteArray()).Replace("-", "", StringComparison.Ordinal)}')";
    }
}
