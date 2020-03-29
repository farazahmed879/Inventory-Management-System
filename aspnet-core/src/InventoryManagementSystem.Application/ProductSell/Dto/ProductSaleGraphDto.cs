﻿using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.ProductSells.Dto
{
    public class ProductSaleGraphDto : FullAuditedEntity<long>
    {
        public string Label { get; set; }
        public string Text { get; set; }
        public double Sale { get; set; }
        public double Profit { get; set; }
    }
}
