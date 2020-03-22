using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class AddedShopProductsSubTypeAndProductSell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubType_TypeId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SubType_Types_TypeId",
                table: "SubType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubType",
                table: "SubType");

            migrationBuilder.RenameTable(
                name: "SubType",
                newName: "SubTypes");

            migrationBuilder.RenameIndex(
                name: "IX_SubType_TypeId",
                table: "SubTypes",
                newName: "IX_SubTypes_TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTypes",
                table: "SubTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopProducts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<long>(nullable: true),
                    CompanyId = table.Column<long>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    WholeSaleRate = table.Column<double>(nullable: true),
                    CompanyRate = table.Column<double>(nullable: true),
                    RetailPrice = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopProducts_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSells",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SellingRate = table.Column<double>(nullable: false),
                    ShopProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSells_ShopProducts_ShopProductId",
                        column: x => x.ShopProductId,
                        principalTable: "ShopProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSells_ShopProductId",
                table: "ProductSells",
                column: "ShopProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopProducts_CompanyId",
                table: "ShopProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopProducts_ProductId",
                table: "ShopProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubTypes_TypeId",
                table: "Products",
                column: "TypeId",
                principalTable: "SubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTypes_Types_TypeId",
                table: "SubTypes",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubTypes_TypeId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTypes_Types_TypeId",
                table: "SubTypes");

            migrationBuilder.DropTable(
                name: "ProductSells");

            migrationBuilder.DropTable(
                name: "ShopProducts");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTypes",
                table: "SubTypes");

            migrationBuilder.RenameTable(
                name: "SubTypes",
                newName: "SubType");

            migrationBuilder.RenameIndex(
                name: "IX_SubTypes_TypeId",
                table: "SubType",
                newName: "IX_SubType_TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubType",
                table: "SubType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubType_TypeId",
                table: "Products",
                column: "TypeId",
                principalTable: "SubType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubType_Types_TypeId",
                table: "SubType",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
