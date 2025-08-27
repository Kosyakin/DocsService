using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocsService.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructionReminderDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderDateOTmarch",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderDateOTseptember",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderDatePBseptember",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderDateOTmarch",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReminderDateOTseptember",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReminderDatePBseptember",
                table: "Users");
        }
    }
}
