﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using InventoryManagementSystem.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventoryManagementSystem.Products
{
    public class Type : FullAuditedEntity<long>, IMayHaveTenant
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }
        public int? TenantId { get; set; }
    }
}
