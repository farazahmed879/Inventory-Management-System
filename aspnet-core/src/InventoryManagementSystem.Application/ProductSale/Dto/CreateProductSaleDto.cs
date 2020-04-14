using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace InventoryManagementSystem.ProductSales.Dto
{
    public class CreateProductSaleDto : Entity<long>
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public double SellingRate { get; set; }
        public long ShopProductId { get; set; }
        public int? Quantity { get; set; }
        public int TenantId { get; set; }

    }
}
