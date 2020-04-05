using Abp.Domain.Entities.Auditing;
using InventoryManagementSystem.Products;
using InventoryManagementSystem.Companies;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Shop
{
    public class ShopProduct : FullAuditedEntity<long>
    {
        public int? Quantity { get; set; }

        [StringLength(100)]
        public string Description { get; set; }
        public double? WholeSaleRate { get; set; }
        public double? CompanyRate { get; set; }
        public double? RetailPrice { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public long ProductId { get; set; }
        public long? CompanyId { get; set; }


    }
}
