using Abp.Domain.Entities.Auditing;
using InventoryManagementSystem.Products;
using InventoryManagementSystem.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagementSystem.Shop
{
    public class ShopProduct : FullAuditedEntity<long>
    {
        public Product Product { get; set; }
        public Company Company { get; set; }
        public int Quantity { get; set; }
        public double? WholeSaleRate { get; set; }
        public double? CompanyRate { get; set; }
        public double? RetailPrice { get; set; }
    }
}
