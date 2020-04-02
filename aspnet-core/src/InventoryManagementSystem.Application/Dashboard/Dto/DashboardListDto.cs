using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace InventoryManagementSystem.Dashboards.Dto
{
    public class DashboardListDto
    {
        public SaleDto Sale { get; set; }
        public SaleDto Costing { get; set; }
        public SaleDto Expenses { get; set; }
        public SaleDto Profit { get; set; }
              
    }
}
