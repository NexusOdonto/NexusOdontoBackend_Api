using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    /// <remarks>
    /// Seed order (parents before children): persons → employees → professionals → users / user_roles /
    /// patients / availabilities / professional_specialties. Each child MERGE uses EXISTS on its parent FK
    /// so a fresh Oracle DB never hits ORA-02291.
    /// </remarks>
    public partial class SeedRemainingUsersPatientsAndAvailabilities : Migration
    {
        private static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime HireDate = new(2026, 1, 15, 0, 0, 0, DateTimeKind.Unspecified);
        private static readonly Guid DocTypeCc = new("e0000000-0000-0000-0000-000000000001");
        private static readonly Guid GenderF = new("f0000000-0000-0000-0000-000000000001");
        private static readonly Guid GenderM = new("f0000000-0000-0000-0000-000000000002");
        private static readonly Guid GenderOther = new("f0000000-0000-0000-0000-000000000003");

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Persons: staff + bot + patients (AdminUserSeeder / UserSeeder / PatientSeeder)
            migrationBuilder.Sql($"""
                MERGE INTO "persons" t
                USING (
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000001"))} AS "id", {Raw(DocTypeCc)} AS "document_type_id", N'1234567890' AS "document_number", N'Carlos' AS "first_name", N'Administrator' AS "last_name", CAST(NULL AS TIMESTAMP(7)) AS "birth_date", CAST(NULL AS RAW(16)) AS "gender_id", CAST(NULL AS NVARCHAR2(30)) AS "phone", N'admin@nexusodonto.com' AS "email", CAST(NULL AS NVARCHAR2(250)) AS "address", 1 AS "is_active", {Ts(SeedDate)} AS "created_at" FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000003"))}, {Raw(DocTypeCc)}, N'1098765432', N'Laura', N'Gómez', CAST(NULL AS TIMESTAMP(7)), CAST(NULL AS RAW(16)), CAST(NULL AS NVARCHAR2(30)), N'odontologo@nexusodonto.com', CAST(NULL AS NVARCHAR2(250)), 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000005"))}, {Raw(DocTypeCc)}, N'1087654321', N'María', N'Rodríguez', CAST(NULL AS TIMESTAMP(7)), CAST(NULL AS RAW(16)), CAST(NULL AS NVARCHAR2(30)), N'recepcion@nexusodonto.com', CAST(NULL AS NVARCHAR2(250)), 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000010"))}, {Raw(DocTypeCc)}, N'1098765433', N'Roberto', N'Martínez', {Ts(new DateTime(1985, 4, 12, 0, 0, 0, DateTimeKind.Utc))}, {Raw(GenderM)}, N'+573001234567', N'roberto.martinez@nexusodonto.com', N'Calle 100 #15-20, Bogotá', 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000011"))}, {Raw(DocTypeCc)}, N'1098765434', N'Ana Sofía', N'Silva', {Ts(new DateTime(1990, 8, 25, 0, 0, 0, DateTimeKind.Utc))}, {Raw(GenderF)}, N'+573002345678', N'ana.silva@nexusodonto.com', N'Carrera 43A #1-50, Medellín', 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("20000000-0000-0000-0000-000000000001"))}, {Raw(DocTypeCc)}, N'BOT-SERVICE-01', N'Chatbot', N'Service', CAST(NULL AS TIMESTAMP(7)), {Raw(GenderOther)}, N'+573000000000', N'bot_service@nexusodonto.com', N'Sistema Interno NexusOdonto', 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000007"))}, {Raw(DocTypeCc)}, N'1076543210', N'Andrés', N'Pérez', CAST(NULL AS TIMESTAMP(7)), CAST(NULL AS RAW(16)), CAST(NULL AS NVARCHAR2(30)), N'paciente@nexusodonto.com', CAST(NULL AS NVARCHAR2(250)), 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000013"))}, {Raw(DocTypeCc)}, N'1076543211', N'Camila', N'López', {Ts(new DateTime(1998, 3, 15, 0, 0, 0, DateTimeKind.Utc))}, {Raw(GenderF)}, N'+573112345678', N'camila.lopez@gmail.com', N'Carrera 7 #72-10, Bogotá', 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000014"))}, {Raw(DocTypeCc)}, N'1076543212', N'Felipe', N'Torres', {Ts(new DateTime(1982, 9, 30, 0, 0, 0, DateTimeKind.Utc))}, {Raw(GenderM)}, N'+573123456789', N'felipe.torres@hotmail.com', N'Calle 53 #45-12, Medellín', 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000015"))}, {Raw(DocTypeCc)}, N'1076543213', N'Valentina', N'Mendoza', {Ts(new DateTime(2002, 6, 20, 0, 0, 0, DateTimeKind.Utc))}, {Raw(GenderF)}, N'+573134567890', N'valentina.mendoza@yahoo.com', N'Avenida Pasoancho #66-10, Cali', 1, {Ts(SeedDate)} FROM dual
                ) s
                ON (t."id" = s."id")
                WHEN NOT MATCHED THEN
                  INSERT ("id", "document_type_id", "document_number", "first_name", "last_name", "birth_date", "gender_id", "phone", "email", "address", "is_active", "created_at")
                  VALUES (s."id", s."document_type_id", s."document_number", s."first_name", s."last_name", s."birth_date", s."gender_id", s."phone", s."email", s."address", s."is_active", s."created_at");
                """);

            // 2. Employees (employee_code exists since AddEmployeeCode 20260901173254)
            migrationBuilder.Sql($"""
                MERGE INTO "employees" t
                USING (
                  SELECT {Raw(new Guid("1c000000-0000-0000-0000-000000000001"))} AS "id", {Raw(new Guid("1b000000-0000-0000-0000-000000000001"))} AS "person_id", {Raw(new Guid("11000000-0000-0000-0000-000000000001"))} AS "job_position_id", N'AD001' AS "employee_code", {Ts(HireDate)} AS "hire_date", 1 AS "is_active", {Ts(SeedDate)} AS "created_at" FROM dual UNION ALL
                  SELECT {Raw(new Guid("1c000000-0000-0000-0000-000000000002"))}, {Raw(new Guid("1b000000-0000-0000-0000-000000000003"))}, {Raw(new Guid("11000000-0000-0000-0000-000000000003"))}, N'OD001', {Ts(HireDate)}, 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1c000000-0000-0000-0000-000000000003"))}, {Raw(new Guid("1b000000-0000-0000-0000-000000000010"))}, {Raw(new Guid("11000000-0000-0000-0000-000000000003"))}, N'OD002', {Ts(HireDate)}, 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1c000000-0000-0000-0000-000000000004"))}, {Raw(new Guid("1b000000-0000-0000-0000-000000000011"))}, {Raw(new Guid("11000000-0000-0000-0000-000000000003"))}, N'OD003', {Ts(HireDate)}, 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1c000000-0000-0000-0000-000000000005"))}, {Raw(new Guid("1b000000-0000-0000-0000-000000000005"))}, {Raw(new Guid("11000000-0000-0000-0000-000000000002"))}, N'RC001', {Ts(HireDate)}, 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1c000000-0000-0000-0000-000000000006"))}, {Raw(new Guid("20000000-0000-0000-0000-000000000001"))}, {Raw(new Guid("11000000-0000-0000-0000-000000000004"))}, N'BOT001', {Ts(HireDate)}, 1, {Ts(SeedDate)} FROM dual
                ) s
                ON (t."id" = s."id")
                WHEN NOT MATCHED AND EXISTS (
                  SELECT 1 FROM "persons" p WHERE p."id" = s."person_id"
                ) THEN
                  INSERT ("id", "person_id", "job_position_id", "employee_code", "hire_date", "is_active", "created_at")
                  VALUES (s."id", s."person_id", s."job_position_id", s."employee_code", s."hire_date", s."is_active", s."created_at");
                """);

            // 3. Professionals
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

            // 4a. Users (hashes from AdminUserSeeder / UserSeeder)
            migrationBuilder.Sql($"""
                MERGE INTO "users" t
                USING (
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000002"))} AS "id", {Raw(new Guid("1b000000-0000-0000-0000-000000000001"))} AS "person_id", N'$2a$11$niKp.bG.Nj.pEEV1ZZY41.MEByC9NN514GjlTZfPuYt.EP/PEO9QG' AS "password_hash", CAST(NULL AS TIMESTAMP(7)) AS "last_access_at", 1 AS "is_active", {Ts(SeedDate)} AS "created_at" FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000004"))}, {Raw(new Guid("1b000000-0000-0000-0000-000000000003"))}, N'$2a$11$XIu0y.lHkWLbOkKZHjlxYerEkr/qEHhqR2qhIY8Q3FkXYbm1pOSN6', CAST(NULL AS TIMESTAMP(7)), 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000006"))}, {Raw(new Guid("1b000000-0000-0000-0000-000000000005"))}, N'$2a$11$VdUVA/jwbm0gDxIOfNs0/OEzCQ.ea2DB48nz7LFJOv1xdkSkhhS9.', CAST(NULL AS TIMESTAMP(7)), 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000008"))}, {Raw(new Guid("1b000000-0000-0000-0000-000000000007"))}, N'$2a$11$wBnMjN.GsYVvbI4loTatCO8H12Ljz9RZbCzbayXngNgRU1WwvLJ1m', CAST(NULL AS TIMESTAMP(7)), 1, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("21000000-0000-0000-0000-000000000001"))}, {Raw(new Guid("20000000-0000-0000-0000-000000000001"))}, N'$2a$11$7exqDQzqUbvt70Ph/7ySpev0OU8YKHaW81fEbDdAyhlUqRKD0Y6mW', CAST(NULL AS TIMESTAMP(7)), 1, {Ts(SeedDate)} FROM dual
                ) s
                ON (t."id" = s."id")
                WHEN NOT MATCHED AND EXISTS (
                  SELECT 1 FROM "persons" p WHERE p."id" = s."person_id"
                ) THEN
                  INSERT ("id", "person_id", "password_hash", "last_access_at", "is_active", "created_at")
                  VALUES (s."id", s."person_id", s."password_hash", s."last_access_at", s."is_active", s."created_at");
                """);

            // 4b. User roles
            migrationBuilder.Sql($"""
                MERGE INTO "user_roles" t
                USING (
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000002"))} AS "user_id", {Raw(new Guid("13000000-0000-0000-0000-000000000001"))} AS "role_id", {Ts(SeedDate)} AS "assigned_at" FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000004"))}, {Raw(new Guid("13000000-0000-0000-0000-000000000002"))}, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000006"))}, {Raw(new Guid("13000000-0000-0000-0000-000000000003"))}, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("1b000000-0000-0000-0000-000000000008"))}, {Raw(new Guid("13000000-0000-0000-0000-000000000005"))}, {Ts(SeedDate)} FROM dual UNION ALL
                  SELECT {Raw(new Guid("21000000-0000-0000-0000-000000000001"))}, {Raw(new Guid("13000000-0000-0000-0000-000000000006"))}, {Ts(SeedDate)} FROM dual
                ) s
                ON (t."user_id" = s."user_id" AND t."role_id" = s."role_id")
                WHEN NOT MATCHED AND EXISTS (
                  SELECT 1 FROM "users" u WHERE u."id" = s."user_id"
                ) THEN
                  INSERT ("user_id", "role_id", "assigned_at")
                  VALUES (s."user_id", s."role_id", s."assigned_at");
                """);

            // 4c. Patients
            migrationBuilder.Sql($"""
                MERGE INTO "patients" t
                USING (
                  SELECT {Raw(new Guid("1e000000-0000-0000-0000-000000000001"))} AS "id", {Raw(new Guid("1b000000-0000-0000-0000-000000000007"))} AS "person_id", N'Lucía Pérez (Madre)' AS "emergency_contact", N'+573101234567' AS "emergency_phone", {Ts(SeedDate)} AS "registration_date", 1 AS "is_active" FROM dual UNION ALL
                  SELECT {Raw(new Guid("1e000000-0000-0000-0000-000000000002"))}, {Raw(new Guid("1b000000-0000-0000-0000-000000000013"))}, N'Jorge López (Padre)', N'+573112345678', {Ts(SeedDate)}, 1 FROM dual UNION ALL
                  SELECT {Raw(new Guid("1e000000-0000-0000-0000-000000000003"))}, {Raw(new Guid("1b000000-0000-0000-0000-000000000014"))}, N'Marta Torres (Esposa)', N'+573123456789', {Ts(SeedDate)}, 1 FROM dual UNION ALL
                  SELECT {Raw(new Guid("1e000000-0000-0000-0000-000000000004"))}, {Raw(new Guid("1b000000-0000-0000-0000-000000000015"))}, N'Carlos Mendoza (Hermano)', N'+573134567890', {Ts(SeedDate)}, 1 FROM dual
                ) s
                ON (t."id" = s."id")
                WHEN NOT MATCHED AND EXISTS (
                  SELECT 1 FROM "persons" p WHERE p."id" = s."person_id"
                ) THEN
                  INSERT ("id", "person_id", "emergency_contact", "emergency_phone", "registration_date", "is_active")
                  VALUES (s."id", s."person_id", s."emergency_contact", s."emergency_phone", s."registration_date", s."is_active");
                """);

            // 4d. Availabilities
            migrationBuilder.Sql($"""
                MERGE INTO "availabilities" t
                USING (
                  SELECT {Raw(new Guid("1f000000-0000-0000-0000-000000000001"))} AS "id", {Raw(new Guid("1d000000-0000-0000-0000-000000000001"))} AS "professional_id", 1 AS "start_day", 5 AS "end_day", N'08:00' AS "start_time", N'12:00' AS "lunch_start_time", N'13:00' AS "lunch_end_time", N'17:00' AS "end_time", 1 AS "is_active" FROM dual UNION ALL
                  SELECT {Raw(new Guid("1f000000-0000-0000-0000-000000000002"))}, {Raw(new Guid("1d000000-0000-0000-0000-000000000001"))}, 6, 6, N'08:00', N'12:00', N'12:00', N'12:00', 1 FROM dual UNION ALL
                  SELECT {Raw(new Guid("1f000000-0000-0000-0000-000000000003"))}, {Raw(new Guid("1d000000-0000-0000-0000-000000000002"))}, 1, 5, N'07:00', N'12:00', N'13:00', N'16:00', 1 FROM dual UNION ALL
                  SELECT {Raw(new Guid("1f000000-0000-0000-0000-000000000004"))}, {Raw(new Guid("1d000000-0000-0000-0000-000000000003"))}, 1, 5, N'09:00', N'13:00', N'14:00', N'18:00', 1 FROM dual
                ) s
                ON (t."id" = s."id")
                WHEN NOT MATCHED AND EXISTS (
                  SELECT 1 FROM "professionals" p WHERE p."id" = s."professional_id"
                ) THEN
                  INSERT ("id", "professional_id", "start_day", "end_day", "start_time", "lunch_start_time", "lunch_end_time", "end_time", "is_active")
                  VALUES (s."id", s."professional_id", s."start_day", s."end_day", s."start_time", s."lunch_start_time", s."lunch_end_time", s."end_time", s."is_active");
                """);

            // 5. Professional specialties (redundant with SeedProfessionalSpecialties; safe after professionals exist)
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
        /// <remarks>
        /// Deletes seeded rows by primary key. Safe when rows were inserted by DatabaseInitializer instead of this migration
        /// (DELETE with no matching rows is a no-op). Does not remove rows that share person/email but use different IDs.
        /// </remarks>
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
                DELETE FROM "availabilities" WHERE "id" IN (
                  {Raw(new Guid("1f000000-0000-0000-0000-000000000001"))},
                  {Raw(new Guid("1f000000-0000-0000-0000-000000000002"))},
                  {Raw(new Guid("1f000000-0000-0000-0000-000000000003"))},
                  {Raw(new Guid("1f000000-0000-0000-0000-000000000004"))}
                );
                """);

            migrationBuilder.Sql($"""
                DELETE FROM "patients" WHERE "id" IN (
                  {Raw(new Guid("1e000000-0000-0000-0000-000000000001"))},
                  {Raw(new Guid("1e000000-0000-0000-0000-000000000002"))},
                  {Raw(new Guid("1e000000-0000-0000-0000-000000000003"))},
                  {Raw(new Guid("1e000000-0000-0000-0000-000000000004"))}
                );
                """);

            // Admin user/role come from AlignPermissionsEnglishAndAdminUser; leave them alone.
            migrationBuilder.Sql($"""
                DELETE FROM "user_roles" WHERE ("user_id", "role_id") IN (
                  ({Raw(new Guid("1b000000-0000-0000-0000-000000000004"))}, {Raw(new Guid("13000000-0000-0000-0000-000000000002"))}),
                  ({Raw(new Guid("1b000000-0000-0000-0000-000000000006"))}, {Raw(new Guid("13000000-0000-0000-0000-000000000003"))}),
                  ({Raw(new Guid("1b000000-0000-0000-0000-000000000008"))}, {Raw(new Guid("13000000-0000-0000-0000-000000000005"))}),
                  ({Raw(new Guid("21000000-0000-0000-0000-000000000001"))}, {Raw(new Guid("13000000-0000-0000-0000-000000000006"))})
                );
                """);

            migrationBuilder.Sql($"""
                DELETE FROM "users" WHERE "id" IN (
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000004"))},
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000006"))},
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000008"))},
                  {Raw(new Guid("21000000-0000-0000-0000-000000000001"))}
                );
                """);

            migrationBuilder.Sql($"""
                DELETE FROM "professionals" WHERE "id" IN (
                  {Raw(new Guid("1d000000-0000-0000-0000-000000000001"))},
                  {Raw(new Guid("1d000000-0000-0000-0000-000000000002"))},
                  {Raw(new Guid("1d000000-0000-0000-0000-000000000003"))}
                );
                """);

            migrationBuilder.Sql($"""
                DELETE FROM "employees" WHERE "id" IN (
                  {Raw(new Guid("1c000000-0000-0000-0000-000000000001"))},
                  {Raw(new Guid("1c000000-0000-0000-0000-000000000002"))},
                  {Raw(new Guid("1c000000-0000-0000-0000-000000000003"))},
                  {Raw(new Guid("1c000000-0000-0000-0000-000000000004"))},
                  {Raw(new Guid("1c000000-0000-0000-0000-000000000005"))},
                  {Raw(new Guid("1c000000-0000-0000-0000-000000000006"))}
                );
                """);

            // Admin person comes from AlignPermissionsEnglishAndAdminUser; leave it alone.
            migrationBuilder.Sql($"""
                DELETE FROM "persons" WHERE "id" IN (
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000003"))},
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000005"))},
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000007"))},
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000010"))},
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000011"))},
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000013"))},
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000014"))},
                  {Raw(new Guid("1b000000-0000-0000-0000-000000000015"))},
                  {Raw(new Guid("20000000-0000-0000-0000-000000000001"))}
                );
                """);
        }

        private static string Raw(Guid id) =>
            $"HEXTORAW('{BitConverter.ToString(id.ToByteArray()).Replace("-", "", StringComparison.Ordinal)}')";

        private static string Ts(DateTime value) =>
            $"TIMESTAMP '{value:yyyy-MM-dd HH:mm:ss.fffffff}'";
    }
}
