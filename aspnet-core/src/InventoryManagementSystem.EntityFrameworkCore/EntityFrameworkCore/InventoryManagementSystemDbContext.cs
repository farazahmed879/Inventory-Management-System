using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using InventoryManagementSystem.Authorization.Roles;
using InventoryManagementSystem.Authorization.Users;
using InventoryManagementSystem.MultiTenancy;

namespace InventoryManagementSystem.EntityFrameworkCore
{
    public class InventoryManagementSystemDbContext : AbpZeroDbContext<Tenant, Role, User, InventoryManagementSystemDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public InventoryManagementSystemDbContext(DbContextOptions<InventoryManagementSystemDbContext> options)
            : base(options)
        {
        }
    }
}
