using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace InventoryManagementSystem.ShopProducts.Dto
{
    public class CreateShopProductDto : Entity<long>
    {
        public string Description { get; set; }
        public double WholeSaleRate { get; set; }
        public int Quantity { get; set; }
        public double? CompanyRate { get; set; }
        public double? RetailPrice { get; set; }
        public long ProductId { get; set; }
        public long? CompanyId { get; set; }
        public long TenantId { get; set; }
    }
}
