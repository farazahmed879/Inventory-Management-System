using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class removeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTypes_Types_TypeId",
                table: "SubTypes");

            migrationBuilder.DropIndex(
                name: "IX_SubTypes_TypeId",
                table: "SubTypes");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "SubTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "SubTypes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubTypes_TypeId",
                table: "SubTypes",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTypes_Types_TypeId",
                table: "SubTypes",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
