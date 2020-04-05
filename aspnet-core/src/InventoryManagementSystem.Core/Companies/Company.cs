using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}
