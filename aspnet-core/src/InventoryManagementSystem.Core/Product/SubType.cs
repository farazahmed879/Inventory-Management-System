using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventoryManagementSystem.Products
{
    public class SubType : FullAuditedEntity<long>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [ForeignKey("ProductTypeId")]
        public Type ProductType { get; set; }
        public long ProductTypeId { get; set; }
    }
}
