using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class AddProductSellRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ShopProductId",
                table: "ProductSells",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSells_ShopProductId",
                table: "ProductSells",
                column: "ShopProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSells_ShopProducts_ShopProductId",
                table: "ProductSells",
                column: "ShopProductId",
                principalTable: "ShopProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSells_ShopProducts_ShopProductId",
                table: "ProductSells");

            migrationBuilder.DropIndex(
                name: "IX_ProductSells_ShopProductId",
                table: "ProductSells");

            migrationBuilder.DropColumn(
                name: "ShopProductId",
                table: "ProductSells");
        }
    }
}
