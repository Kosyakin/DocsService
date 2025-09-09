using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocsService.Migrations
{
    /// <inheritdoc />
    public partial class Dates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 2,
                column: "Email_User",
                value: "login.00@gmail.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 2,
                column: "Email_User",
                value: "kurdina.00@gmail.com");
        }
    }
}
