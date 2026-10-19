using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVariantSku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "product_variants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sku",
                table: "product_variants",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "sku",
                table: "product_variants");
        }
    }
}
