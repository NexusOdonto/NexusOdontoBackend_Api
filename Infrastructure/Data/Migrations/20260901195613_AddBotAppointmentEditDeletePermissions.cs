using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBotAppointmentEditDeletePermissions : Migration
    {
        private static readonly Guid BotServiceRoleId = new("13000000-0000-0000-0000-000000000006");
        private static readonly Guid AppointmentsEditPermissionId = new("14000000-0000-0000-0002-000000000003");
        private static readonly Guid AppointmentsDeletePermissionId = new("14000000-0000-0000-0002-000000000004");

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Idempotent Oracle MERGE: safe when DatabaseInitializer already assigned these permissions to BOT_SERVICE.
            migrationBuilder.Sql($"""
                MERGE INTO "role_permissions" t
                USING (
                  SELECT {Raw(AppointmentsEditPermissionId)} AS "permission_id", {Raw(BotServiceRoleId)} AS "role_id" FROM dual UNION ALL
                  SELECT {Raw(AppointmentsDeletePermissionId)}, {Raw(BotServiceRoleId)} FROM dual
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
                  ({Raw(AppointmentsEditPermissionId)}, {Raw(BotServiceRoleId)}),
                  ({Raw(AppointmentsDeletePermissionId)}, {Raw(BotServiceRoleId)})
                );
                """);
        }

        private static string Raw(Guid id) =>
            $"HEXTORAW('{BitConverter.ToString(id.ToByteArray()).Replace("-", "", StringComparison.Ordinal)}')";
    }
}
