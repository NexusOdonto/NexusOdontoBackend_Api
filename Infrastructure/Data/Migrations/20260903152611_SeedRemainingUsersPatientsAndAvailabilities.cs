using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRemainingUsersPatientsAndAvailabilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var docTypeCc = new Guid("e0000000-0000-0000-0000-000000000001");
            var genderF = new Guid("f0000000-0000-0000-0000-000000000001");
            var genderM = new Guid("f0000000-0000-0000-0000-000000000002");

            // 1. Personas faltantes (Pacientes)
            migrationBuilder.InsertData(
                table: "persons",
                columns: new[] { "id", "document_type_id", "document_number", "first_name", "last_name", "birth_date", "gender_id", "phone", "email", "address", "is_active", "created_at" },
                values: new object[,]
                {
                    { new Guid("1b000000-0000-0000-0000-000000000007"), docTypeCc, "1076543210", "Andrés", "Pérez", null, null, null, "paciente@nexusodonto.com", null, true, seedDate },
                    { new Guid("1b000000-0000-0000-0000-000000000013"), docTypeCc, "1076543211", "Camila", "López", new DateTime(1998, 3, 15, 0, 0, 0, DateTimeKind.Utc), genderF, "+573112345678", "camila.lopez@gmail.com", "Carrera 7 #72-10, Bogotá", true, seedDate },
                    { new Guid("1b000000-0000-0000-0000-000000000014"), docTypeCc, "1076543212", "Felipe", "Torres", new DateTime(1982, 9, 30, 0, 0, 0, DateTimeKind.Utc), genderM, "+573123456789", "felipe.torres@hotmail.com", "Calle 53 #45-12, Medellín", true, seedDate },
                    { new Guid("1b000000-0000-0000-0000-000000000015"), docTypeCc, "1076543213", "Valentina", "Mendoza", new DateTime(2002, 6, 20, 0, 0, 0, DateTimeKind.Utc), genderF, "+573134567890", "valentina.mendoza@yahoo.com", "Avenida Pasoancho #66-10, Cali", true, seedDate }
                });

            // 2. Usuarios faltantes
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "person_id", "password_hash", "last_access_at", "is_active", "created_at" },
                values: new object[,]
                {
                    { new Guid("1b000000-0000-0000-0000-000000000004"), new Guid("1b000000-0000-0000-0000-000000000003"), "$2a$11$XIu0y.lHkWLbOkKZHjlxYerEkr/qEHhqR2qhIY8Q3FkXYbm1pOSN6", null, true, seedDate },
                    { new Guid("1b000000-0000-0000-0000-000000000006"), new Guid("1b000000-0000-0000-0000-000000000005"), "$2a$11$VdUVA/jwbm0gDxIOfNs0/OEzCQ.ea2DB48nz7LFJOv1xdkSkhhS9.", null, true, seedDate },
                    { new Guid("1b000000-0000-0000-0000-000000000008"), new Guid("1b000000-0000-0000-0000-000000000007"), "$2a$11$wBnMjN.GsYVvbI4loTatCO8H12Ljz9RZbCzbayXngNgRU1WwvLJ1m", null, true, seedDate },
                    { new Guid("21000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001"), "$2a$11$7exqDQzqUbvt70Ph/7ySpev0OU8YKHaW81fEbDdAyhlUqRKD0Y6mW", null, true, seedDate }
                });

            // 3. Asignación de roles a usuarios
            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "user_id", "role_id", "assigned_at" },
                values: new object[,]
                {
                    { new Guid("1b000000-0000-0000-0000-000000000004"), new Guid("13000000-0000-0000-0000-000000000002"), seedDate },
                    { new Guid("1b000000-0000-0000-0000-000000000006"), new Guid("13000000-0000-0000-0000-000000000003"), seedDate },
                    { new Guid("1b000000-0000-0000-0000-000000000008"), new Guid("13000000-0000-0000-0000-000000000005"), seedDate },
                    { new Guid("21000000-0000-0000-0000-000000000001"), new Guid("13000000-0000-0000-0000-000000000006"), seedDate }
                });

            // 4. Pacientes
            migrationBuilder.InsertData(
                table: "patients",
                columns: new[] { "id", "person_id", "emergency_contact", "emergency_phone", "registration_date", "is_active" },
                values: new object[,]
                {
                    { new Guid("1e000000-0000-0000-0000-000000000001"), new Guid("1b000000-0000-0000-0000-000000000007"), "Lucía Pérez (Madre)", "+573101234567", seedDate, true },
                    { new Guid("1e000000-0000-0000-0000-000000000002"), new Guid("1b000000-0000-0000-0000-000000000013"), "Jorge López (Padre)", "+573112345678", seedDate, true },
                    { new Guid("1e000000-0000-0000-0000-000000000003"), new Guid("1b000000-0000-0000-0000-000000000014"), "Marta Torres (Esposa)", "+573123456789", seedDate, true },
                    { new Guid("1e000000-0000-0000-0000-000000000004"), new Guid("1b000000-0000-0000-0000-000000000015"), "Carlos Mendoza (Hermano)", "+573134567890", seedDate, true }
                });

            // 5. Horarios de atención (Availabilities)
            migrationBuilder.InsertData(
                table: "availabilities",
                columns: new[] { "id", "professional_id", "start_day", "end_day", "start_time", "lunch_start_time", "lunch_end_time", "end_time", "is_active" },
                values: new object[,]
                {
                    { new Guid("1f000000-0000-0000-0000-000000000001"), new Guid("1d000000-0000-0000-0000-000000000001"), 1, 5, "08:00", "12:00", "13:00", "17:00", true },
                    { new Guid("1f000000-0000-0000-0000-000000000002"), new Guid("1d000000-0000-0000-0000-000000000001"), 6, 6, "08:00", "12:00", "12:00", "12:00", true },
                    { new Guid("1f000000-0000-0000-0000-000000000003"), new Guid("1d000000-0000-0000-0000-000000000002"), 1, 5, "07:00", "12:00", "13:00", "16:00", true },
                    { new Guid("1f000000-0000-0000-0000-000000000004"), new Guid("1d000000-0000-0000-0000-000000000003"), 1, 5, "09:00", "13:00", "14:00", "18:00", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "availabilities", keyColumn: "id", keyValue: new Guid("1f000000-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "availabilities", keyColumn: "id", keyValue: new Guid("1f000000-0000-0000-0000-000000000002"));
            migrationBuilder.DeleteData(table: "availabilities", keyColumn: "id", keyValue: new Guid("1f000000-0000-0000-0000-000000000003"));
            migrationBuilder.DeleteData(table: "availabilities", keyColumn: "id", keyValue: new Guid("1f000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(table: "patients", keyColumn: "id", keyValue: new Guid("1e000000-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "patients", keyColumn: "id", keyValue: new Guid("1e000000-0000-0000-0000-000000000002"));
            migrationBuilder.DeleteData(table: "patients", keyColumn: "id", keyValue: new Guid("1e000000-0000-0000-0000-000000000003"));
            migrationBuilder.DeleteData(table: "patients", keyColumn: "id", keyValue: new Guid("1e000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(table: "user_roles", keyColumns: new[] { "user_id", "role_id" }, keyValues: new object[] { new Guid("1b000000-0000-0000-0000-000000000004"), new Guid("13000000-0000-0000-0000-000000000002") });
            migrationBuilder.DeleteData(table: "user_roles", keyColumns: new[] { "user_id", "role_id" }, keyValues: new object[] { new Guid("1b000000-0000-0000-0000-000000000006"), new Guid("13000000-0000-0000-0000-000000000003") });
            migrationBuilder.DeleteData(table: "user_roles", keyColumns: new[] { "user_id", "role_id" }, keyValues: new object[] { new Guid("1b000000-0000-0000-0000-000000000008"), new Guid("13000000-0000-0000-0000-000000000005") });
            migrationBuilder.DeleteData(table: "user_roles", keyColumns: new[] { "user_id", "role_id" }, keyValues: new object[] { new Guid("21000000-0000-0000-0000-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(table: "users", keyColumn: "id", keyValue: new Guid("1b000000-0000-0000-0000-000000000004"));
            migrationBuilder.DeleteData(table: "users", keyColumn: "id", keyValue: new Guid("1b000000-0000-0000-0000-000000000006"));
            migrationBuilder.DeleteData(table: "users", keyColumn: "id", keyValue: new Guid("1b000000-0000-0000-0000-000000000008"));
            migrationBuilder.DeleteData(table: "users", keyColumn: "id", keyValue: new Guid("21000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(table: "persons", keyColumn: "id", keyValue: new Guid("1b000000-0000-0000-0000-000000000007"));
            migrationBuilder.DeleteData(table: "persons", keyColumn: "id", keyValue: new Guid("1b000000-0000-0000-0000-000000000013"));
            migrationBuilder.DeleteData(table: "persons", keyColumn: "id", keyValue: new Guid("1b000000-0000-0000-0000-000000000014"));
            migrationBuilder.DeleteData(table: "persons", keyColumn: "id", keyValue: new Guid("1b000000-0000-0000-0000-000000000015"));
        }
    }
}
