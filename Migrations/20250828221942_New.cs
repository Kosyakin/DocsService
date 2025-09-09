using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocsService.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OTmarch",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OTseptember",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PBseptember",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTmarch",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OTseptember",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PBseptember",
                table: "Users");
        }
    }
}
