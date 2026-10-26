using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSortOrderToVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "product_variants",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "product_variants");
        }
    }
}
