using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeCodeToEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                BEGIN
                  EXECUTE IMMEDIATE 'ALTER TABLE "employees" ADD "employee_code" NVARCHAR2(10)';
                EXCEPTION
                  WHEN OTHERS THEN
                    IF SQLCODE != -1430 THEN RAISE; END IF;
                END;
                """);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("1c000000-0000-0000-0000-000000000001"),
                column: "employee_code",
                value: "AD001");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("1c000000-0000-0000-0000-000000000002"),
                column: "employee_code",
                value: "OD001");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("1c000000-0000-0000-0000-000000000003"),
                column: "employee_code",
                value: "OD002");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("1c000000-0000-0000-0000-000000000004"),
                column: "employee_code",
                value: "OD003");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("1c000000-0000-0000-0000-000000000005"),
                column: "employee_code",
                value: "RC001");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("1c000000-0000-0000-0000-000000000006"),
                columns: ["employee_code", "person_id", "job_position_id"],
                values: ["BOT001", new Guid("20000000-0000-0000-0000-000000000001"), new Guid("11000000-0000-0000-0000-000000000004")]);

            migrationBuilder.Sql("""
                BEGIN
                  EXECUTE IMMEDIATE 'ALTER TABLE "employees" MODIFY "employee_code" NVARCHAR2(10) NOT NULL';
                EXCEPTION
                  WHEN OTHERS THEN
                    IF SQLCODE != -1442 THEN RAISE; END IF;
                END;
                """);

            migrationBuilder.Sql("""
                BEGIN
                  EXECUTE IMMEDIATE 'CREATE UNIQUE INDEX "IX_employees_employee_code" ON "employees" ("employee_code")';
                EXCEPTION
                  WHEN OTHERS THEN
                    IF SQLCODE != -955 AND SQLCODE != -1408 THEN RAISE; END IF;
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                BEGIN
                  EXECUTE IMMEDIATE 'DROP INDEX "IX_employees_employee_code"';
                EXCEPTION
                  WHEN OTHERS THEN
                    IF SQLCODE != -1418 THEN RAISE; END IF;
                END;
                """);

            migrationBuilder.DropColumn(
                name: "employee_code",
                table: "employees");
        }
    }
}
