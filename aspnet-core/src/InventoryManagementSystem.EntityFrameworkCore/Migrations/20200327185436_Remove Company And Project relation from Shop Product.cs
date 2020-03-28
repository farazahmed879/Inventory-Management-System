using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class RemoveCompanyAndProjectrelationfromShopProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_Company_CompanyId",
                table: "ShopProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_Products_ProductId",
                table: "ShopProducts");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_ShopProducts_CompanyId",
                table: "ShopProducts");

            migrationBuilder.DropIndex(
                name: "IX_ShopProducts_ProductId",
                table: "ShopProducts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ShopProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShopProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "ShopProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ShopProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopProducts_CompanyId",
                table: "ShopProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopProducts_ProductId",
                table: "ShopProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_Company_CompanyId",
                table: "ShopProducts",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_Products_ProductId",
                table: "ShopProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
