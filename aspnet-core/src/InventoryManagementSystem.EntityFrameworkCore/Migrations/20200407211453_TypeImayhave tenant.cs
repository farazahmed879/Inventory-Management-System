using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class TypeImayhavetenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "Types",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                table: "Types",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
