using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace InventoryManagementSystem.SubTypes.Dto
{
    public class CreateSubTypeDto :Entity<long>
    {
        public string Name { get; set; }
        public long ProductTypeId { get; set; }
    }
}
