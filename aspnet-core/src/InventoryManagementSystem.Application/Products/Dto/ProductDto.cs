﻿using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.Products.Dto
{
   public class ProductDto : FullAuditedEntity<long>
    {
        public string Name { get; set; }
    }
}