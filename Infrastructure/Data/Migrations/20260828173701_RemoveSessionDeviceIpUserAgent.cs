using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSessionDeviceIpUserAgent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "device",
                table: "sessions");

            migrationBuilder.DropColumn(
                name: "ip_address",
                table: "sessions");

            migrationBuilder.DropColumn(
                name: "user_agent",
                table: "sessions");

            migrationBuilder.DropColumn(
                name: "ip_address",
                table: "audit_events");

            migrationBuilder.DropColumn(
                name: "user_agent",
                table: "audit_events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "device",
                table: "sessions",
                type: "NVARCHAR2(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ip_address",
                table: "sessions",
                type: "NVARCHAR2(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "user_agent",
                table: "sessions",
                type: "NVARCHAR2(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ip_address",
                table: "audit_events",
                type: "NVARCHAR2(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "user_agent",
                table: "audit_events",
                type: "NVARCHAR2(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
