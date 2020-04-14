using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class AddTenantRelationWithAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "SubTypes",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "ShopProducts",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "ProductSells",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "Expenses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SubTypes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ShopProducts");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductSells");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Expenses");
        }
    }
}
