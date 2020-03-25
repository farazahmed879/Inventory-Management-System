using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace InventoryManagementSystem.ProductSells.Dto
{
    public class CreateProductSellDto : Entity<long>
    {
        public string Status { get; set; }
        public double SellingRate { get; set; }
    }
}
