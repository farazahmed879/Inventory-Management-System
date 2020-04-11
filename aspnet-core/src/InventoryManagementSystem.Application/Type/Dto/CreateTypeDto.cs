using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace InventoryManagementSystem.Types.Dto
{
    public class CreateTypeDto :Entity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long TenantId { get; set; }
    }
}
