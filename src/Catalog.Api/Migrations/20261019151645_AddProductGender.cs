using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProductGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gender",
                table: "products");
        }
    }
}
