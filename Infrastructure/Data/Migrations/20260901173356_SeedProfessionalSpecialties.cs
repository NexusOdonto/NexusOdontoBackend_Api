using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedProfessionalSpecialties : Migration
    {
        private static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Parents first (idempotent): only insert professionals when the employee row already exists.
            // SeedRemaining later MERGEs persons/employees if this runs on a fresh DB before they exist.
            migrationBuilder.Sql($"""
                MERGE INTO "professionals" t
                USING (
                  SELECT {Raw(new Guid("1d000000-0000-0000-0000-000000000001"))} AS "id", {Raw(new Guid("1c000000-0000-0000-0000-000000000002"))} AS "employee_id", N'TP-OD-1098765' AS "professional_license", 1 AS "is_active", {Ts(SeedDate)} AS "created_at" FROM dual UNION ALL
                  SELECT {Raw(new Guid("1d000000-0000-0000-0000-000000000002"))}, {Raw(new Guid("1c000000-0000-0000-0000-000000000003"))}, N'TP-OD-1098766', 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1d000000-0000-0000-0000-000000000003"))}, {Raw(new Guid("1c000000-0000-0000-0000-000000000004"))}, N'TP-OD-1098767', 1, {Ts(SeedDate)} FROM dual
                ) s
                ON (t."id" = s."id")
                WHEN NOT MATCHED AND EXISTS (
                  SELECT 1 FROM "employees" e WHERE e."id" = s."employee_id"
                ) THEN
                  INSERT ("id", "employee_id", "professional_license", "is_active", "created_at")
                  VALUES (s."id", s."employee_id", s."professional_license", s."is_active", s."created_at");
                """);

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
                WHEN NOT MATCHED AND EXISTS (
                  SELECT 1 FROM "professionals" p WHERE p."id" = s."professional_id"
                ) THEN
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

            migrationBuilder.Sql($"""
                DELETE FROM "professionals" WHERE "id" IN (
                  {Raw(new Guid("1d000000-0000-0000-0000-000000000001"))},
                  {Raw(new Guid("1d000000-0000-0000-0000-000000000002"))},
                  {Raw(new Guid("1d000000-0000-0000-0000-000000000003"))}
                );
                """);
        }

        private static string Raw(Guid id) =>
            $"HEXTORAW('{BitConverter.ToString(id.ToByteArray()).Replace("-", "", StringComparison.Ordinal)}')";

        private static string Ts(DateTime value) =>
            $"TIMESTAMP '{value:yyyy-MM-dd HH:mm:ss.fffffff}'";
    }
}
