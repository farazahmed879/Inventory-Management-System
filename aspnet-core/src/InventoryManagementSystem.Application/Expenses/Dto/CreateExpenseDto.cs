using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace InventoryManagementSystem.Expenses.Dto
{
    public class CreateExpenseDto : Entity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public int TenantId { get; set; }
    }
}
