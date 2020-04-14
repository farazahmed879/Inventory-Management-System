using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace InventoryManagementSystem.Dashboards.Dto
{
    public class SaleDto
    {
        public double Today { get; set; }
        public double Yesterday { get; set; }
        public double ThisWeek { get; set; }
        public double ThisMonth { get; set; }
        public double ThisYear { get; set; }
       
       
    }
}
