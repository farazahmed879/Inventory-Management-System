﻿using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace InventoryManagementSystem.ShopProducts.Dto
{
    public class CreateShopProductDto : Entity<long>
    {
        public double WholeSaleRate { get; set; }
        public int Quantity { get; set; }
        public double? CompanyRate { get; set; }
        public double? RetailPrice { get; set; }
    }
}
