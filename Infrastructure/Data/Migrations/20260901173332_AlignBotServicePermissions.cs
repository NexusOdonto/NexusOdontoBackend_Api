using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlignBotServicePermissions : Migration
    {
        private static readonly Guid BotServiceRoleId = new("13000000-0000-0000-0000-000000000006");

        private static readonly Guid[] BotMinimalPermissionIds =
        [
            new Guid("14000000-0000-0000-0001-000000000001"),
            new Guid("14000000-0000-0000-0001-000000000002"),
            new Guid("14000000-0000-0000-0002-000000000001"),
            new Guid("14000000-0000-0000-0002-000000000002"),
            new Guid("14000000-0000-0000-0010-000000000001")
        ];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (var moduleIndex = 1; moduleIndex <= 12; moduleIndex++)
            {
                for (var actionIndex = 1; actionIndex <= 4; actionIndex++)
                {
                    var permissionId = new Guid($"14000000-0000-0000-{moduleIndex:D4}-{actionIndex:D12}");
                    migrationBuilder.DeleteData(
                        table: "role_permissions",
                        keyColumns: ["permission_id", "role_id"],
                        keyValues: [permissionId, BotServiceRoleId]);
                }
            }

            foreach (var permissionId in BotMinimalPermissionIds)
            {
                migrationBuilder.InsertData(
                    table: "role_permissions",
                    columns: ["permission_id", "role_id"],
                    values: [permissionId, BotServiceRoleId]);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var permissionId in BotMinimalPermissionIds)
            {
                migrationBuilder.DeleteData(
                    table: "role_permissions",
                    keyColumns: ["permission_id", "role_id"],
                    keyValues: [permissionId, BotServiceRoleId]);
            }
        }
    }
}
