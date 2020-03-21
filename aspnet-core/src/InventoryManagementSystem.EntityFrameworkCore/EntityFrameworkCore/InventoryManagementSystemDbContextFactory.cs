using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using InventoryManagementSystem.Configuration;
using InventoryManagementSystem.Web;

namespace InventoryManagementSystem.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class InventoryManagementSystemDbContextFactory : IDesignTimeDbContextFactory<InventoryManagementSystemDbContext>
    {
        public InventoryManagementSystemDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<InventoryManagementSystemDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            InventoryManagementSystemDbContextConfigurer.Configure(builder, configuration.GetConnectionString(InventoryManagementSystemConsts.ConnectionStringName));

            return new InventoryManagementSystemDbContext(builder.Options);
        }
    }
}
