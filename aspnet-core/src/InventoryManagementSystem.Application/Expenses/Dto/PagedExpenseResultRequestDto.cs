using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Abp.Application.Services.Dto;

namespace InventoryManagementSystem.Expenses.Dto
{
    public class PagedExpenseResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public long? TenantId { get; set; }
    }
}
