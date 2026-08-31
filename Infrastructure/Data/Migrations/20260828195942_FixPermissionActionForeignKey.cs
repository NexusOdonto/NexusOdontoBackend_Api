using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPermissionActionForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_permissions_permission_actions_ActionPermissionEntityId",
                table: "permissions");

            migrationBuilder.DropIndex(
                name: "IX_permissions_ActionPermissionEntityId",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "ActionPermissionEntityId",
                table: "permissions");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_permission_action_id",
                table: "permissions",
                column: "permission_action_id");

            migrationBuilder.AddForeignKey(
                name: "FK_permissions_permission_actions_permission_action_id",
                table: "permissions",
                column: "permission_action_id",
                principalTable: "permission_actions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_permissions_permission_actions_permission_action_id",
                table: "permissions");

            migrationBuilder.DropIndex(
                name: "IX_permissions_permission_action_id",
                table: "permissions");

            migrationBuilder.AddColumn<Guid>(
                name: "ActionPermissionEntityId",
                table: "permissions",
                type: "RAW(16)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_permissions_ActionPermissionEntityId",
                table: "permissions",
                column: "ActionPermissionEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_permissions_permission_actions_ActionPermissionEntityId",
                table: "permissions",
                column: "ActionPermissionEntityId",
                principalTable: "permission_actions",
                principalColumn: "id");
        }
    }
}
