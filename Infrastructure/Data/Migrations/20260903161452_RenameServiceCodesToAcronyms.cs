using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameServiceCodesToAcronyms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                column: "code",
                value: "GA");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"),
                column: "code",
                value: "DP");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"),
                column: "code",
                value: "CRF");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"),
                column: "code",
                value: "TW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                column: "code",
                value: "VALORACION_GENERAL");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"),
                column: "code",
                value: "PROFILAXIS");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"),
                column: "code",
                value: "CALZA_RESINA");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"),
                column: "code",
                value: "BLANQUEAMIENTO");
        }
    }
}
