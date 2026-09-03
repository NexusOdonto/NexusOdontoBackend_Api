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
                    migrationBuilder.Sql($"""
                        DELETE FROM "role_permissions"
                        WHERE "permission_id" = {Raw(permissionId)}
                          AND "role_id" = {Raw(BotServiceRoleId)};
                        """);
                }
            }

            // Idempotent Oracle MERGE: safe when DatabaseInitializer already assigned minimal permissions to BOT_SERVICE.
            migrationBuilder.Sql($"""
                MERGE INTO "role_permissions" t
                USING (
                  SELECT {Raw(BotMinimalPermissionIds[0])} AS "permission_id", {Raw(BotServiceRoleId)} AS "role_id" FROM dual UNION ALL
                  SELECT {Raw(BotMinimalPermissionIds[1])}, {Raw(BotServiceRoleId)} FROM dual UNION ALL
                  SELECT {Raw(BotMinimalPermissionIds[2])}, {Raw(BotServiceRoleId)} FROM dual UNION ALL
                  SELECT {Raw(BotMinimalPermissionIds[3])}, {Raw(BotServiceRoleId)} FROM dual UNION ALL
                  SELECT {Raw(BotMinimalPermissionIds[4])}, {Raw(BotServiceRoleId)} FROM dual
                ) s
                ON (t."permission_id" = s."permission_id" AND t."role_id" = s."role_id")
                WHEN NOT MATCHED THEN
                  INSERT ("permission_id", "role_id")
                  VALUES (s."permission_id", s."role_id");
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                DELETE FROM "role_permissions"
                WHERE ("permission_id", "role_id") IN (
                  ({Raw(BotMinimalPermissionIds[0])}, {Raw(BotServiceRoleId)}),
                  ({Raw(BotMinimalPermissionIds[1])}, {Raw(BotServiceRoleId)}),
                  ({Raw(BotMinimalPermissionIds[2])}, {Raw(BotServiceRoleId)}),
                  ({Raw(BotMinimalPermissionIds[3])}, {Raw(BotServiceRoleId)}),
                  ({Raw(BotMinimalPermissionIds[4])}, {Raw(BotServiceRoleId)})
                );
                """);
        }

        private static string Raw(Guid id) =>
            $"HEXTORAW('{BitConverter.ToString(id.ToByteArray()).Replace("-", "", StringComparison.Ordinal)}')";
    }
}
