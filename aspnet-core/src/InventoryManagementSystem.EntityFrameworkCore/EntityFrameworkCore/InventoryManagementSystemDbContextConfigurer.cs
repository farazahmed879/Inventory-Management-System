using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.EntityFrameworkCore
{
    public static class InventoryManagementSystemDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<InventoryManagementSystemDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<InventoryManagementSystemDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
