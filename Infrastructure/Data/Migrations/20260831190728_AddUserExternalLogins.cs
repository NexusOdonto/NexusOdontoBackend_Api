using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserExternalLogins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                BEGIN
                  EXECUTE IMMEDIATE 'DROP TABLE "user_external_logins" CASCADE CONSTRAINTS';
                EXCEPTION
                  WHEN OTHERS THEN
                    IF SQLCODE != -942 THEN RAISE; END IF;
                END;
                """);

            migrationBuilder.CreateTable(
                name: "user_external_logins",
                columns: table => new
                {
                    provider = table.Column<string>(type: "NVARCHAR2(32)", maxLength: 32, nullable: false),
                    provider_key = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    user_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    provider_email = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    linked_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_external_logins", x => new { x.provider, x.provider_key });
                    table.ForeignKey(
                        name: "FK_user_external_logins_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0001-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0001-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0002-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0002-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0002-000000000003"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0003-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0004-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0007-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0007-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0008-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0008-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0008-000000000003"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0011-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("13000000-0000-0000-0000-000000000006"));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "code", "created_at", "description", "is_active", "name", "updated_at" },
                values: new object[] { new Guid("13000000-0000-0000-0000-000000000006"), "BOT_SERVICE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Service role for automated Chatbot integration with minimum required privileges.", true, "Bot Service", null });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "permission_id", "role_id" },
                values: new object[,]
                {
                    { new Guid("14000000-0000-0000-0001-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0001-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0002-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0002-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0002-000000000003"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0003-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0004-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0007-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0007-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0008-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0008-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0008-000000000003"), new Guid("13000000-0000-0000-0000-000000000006") },
                    { new Guid("14000000-0000-0000-0011-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_external_logins_user_id",
                table: "user_external_logins",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_external_logins");

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0001-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0001-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0002-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0002-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0002-000000000003"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0003-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0004-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0007-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0007-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0008-000000000001"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0008-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0008-000000000003"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0011-000000000002"), new Guid("13000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("13000000-0000-0000-0000-000000000006"));
        }
    }
}
