using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagementSystem.Expenses
{
    public class Expense : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
    }
}
