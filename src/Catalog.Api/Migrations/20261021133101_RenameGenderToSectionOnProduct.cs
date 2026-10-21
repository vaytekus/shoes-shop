using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameGenderToSectionOnProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gender",
                table: "products");

            migrationBuilder.AddColumn<string>(
                name: "section",
                table: "products",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "section",
                table: "products");

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
