using Abp.Domain.Entities.Auditing;
using InventoryManagementSystem.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventoryManagementSystem.Companies
{
    public class Company : FullAuditedEntity<long>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        [StringLength(100)]
        public string Description { get; set; }

        //[ForeignKey("TenantId")]
        //public Tenant Tenant { get; set; }

        public long? TenantId { get; set; }
    }
}
