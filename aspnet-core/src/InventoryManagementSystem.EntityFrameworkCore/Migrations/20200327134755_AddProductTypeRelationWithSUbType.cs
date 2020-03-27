using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class AddProductTypeRelationWithSUbType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductTypeId",
                table: "SubTypes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SubTypes_ProductTypeId",
                table: "SubTypes",
                column: "ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTypes_Types_ProductTypeId",
                table: "SubTypes",
                column: "ProductTypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTypes_Types_ProductTypeId",
                table: "SubTypes");

            migrationBuilder.DropIndex(
                name: "IX_SubTypes_ProductTypeId",
                table: "SubTypes");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "SubTypes");
        }
    }
}
