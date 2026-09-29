using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAvailabilityEffectiveDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "effective_from",
                table: "availabilities",
                type: "DATE",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "effective_to",
                table: "availabilities",
                type: "DATE",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "effective_from",
                table: "availabilities");

            migrationBuilder.DropColumn(
                name: "effective_to",
                table: "availabilities");
        }
    }
}
