using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBotAppointmentEditDeletePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "permission_id", "role_id" },
                values: new object[,]
                {
                    { new Guid("14000000-0000-0000-0002-000000000003"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0002-000000000004"), new Guid("13000000-0000-0000-0000-000000000006") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0002-000000000003"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0002-000000000004"), new Guid("13000000-0000-0000-0000-000000000006") });
        }
    }
}
