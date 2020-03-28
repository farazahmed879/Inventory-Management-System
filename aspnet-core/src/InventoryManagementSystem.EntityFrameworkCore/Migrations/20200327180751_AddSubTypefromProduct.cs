using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class AddSubTypefromProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductSubTypeId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductSubTypeId",
                table: "Products",
                column: "ProductSubTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubTypes_ProductSubTypeId",
                table: "Products",
                column: "ProductSubTypeId",
                principalTable: "SubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubTypes_ProductSubTypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductSubTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductSubTypeId",
                table: "Products");
        }
    }
}
