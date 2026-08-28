using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRoleForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_roles_RoleEntityId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_users_UserEntityId",
                table: "user_roles");

            migrationBuilder.DropIndex(
                name: "IX_user_roles_RoleEntityId",
                table: "user_roles");

            migrationBuilder.DropIndex(
                name: "IX_user_roles_UserEntityId",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "RoleEntityId",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "user_roles");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_roles_role_id",
                table: "user_roles",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_users_user_id",
                table: "user_roles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_roles_role_id",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_users_user_id",
                table: "user_roles");

            migrationBuilder.DropIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleEntityId",
                table: "user_roles",
                type: "RAW(16)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "user_roles",
                type: "RAW(16)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { new Guid("13000000-0000-0000-0000-000000000001"), new Guid("1b000000-0000-0000-0000-000000000002") },
                columns: new[] { "RoleEntityId", "UserEntityId" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_RoleEntityId",
                table: "user_roles",
                column: "RoleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_UserEntityId",
                table: "user_roles",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_roles_RoleEntityId",
                table: "user_roles",
                column: "RoleEntityId",
                principalTable: "roles",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_users_UserEntityId",
                table: "user_roles",
                column: "UserEntityId",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
