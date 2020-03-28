using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventoryManagementSystem.Shop
{
    public class ProductSell : FullAuditedEntity<long>
    {
        public string Status { get; set; }
        public double SellingRate { get; set; }

        [ForeignKey("ShopProductId")]
        public ShopProduct ShopProduct { get; set; }
        public long ShopProductId { get; set; }
    }
}
