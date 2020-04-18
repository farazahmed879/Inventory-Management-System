using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using InventoryManagementSystem.Authorization.Roles;
using InventoryManagementSystem.Authorization.Users;
using InventoryManagementSystem.Companies;
using InventoryManagementSystem.MultiTenancy;
using InventoryManagementSystem.Products;
using InventoryManagementSystem.Shop;
using InventoryManagementSystem.Expenses;

namespace InventoryManagementSystem.EntityFrameworkCore
{
    public class InventoryManagementSystemDbContext : AbpZeroDbContext<Tenant, Role, User, InventoryManagementSystemDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SubType> SubTypes { get; set; }
        public DbSet<ShopProduct> ShopProducts { get; set; }
        public DbSet<ProductSell> ProductSells { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public InventoryManagementSystemDbContext(DbContextOptions<InventoryManagementSystemDbContext> options)
            : base(options)
        {


        }
    }
}
