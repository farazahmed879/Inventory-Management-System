using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Abp.Application.Services.Dto;

namespace InventoryManagementSystem.ProductSells.Dto
{
    public class PagedProductSellResultRequestDto : PagedResultRequestDto
    {
        public string Status { get; set; }
        public double SellingRate { get; set; }
        public long ShopProductId { get; set; }
        public string ProductName { get; set; }
    }
}
