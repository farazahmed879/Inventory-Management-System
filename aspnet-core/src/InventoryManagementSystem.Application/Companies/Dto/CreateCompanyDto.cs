using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace InventoryManagementSystem.Companies.Dto
{
    public class CreateCompanyDto :Entity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long TenantId { get; set; }
    }
}
