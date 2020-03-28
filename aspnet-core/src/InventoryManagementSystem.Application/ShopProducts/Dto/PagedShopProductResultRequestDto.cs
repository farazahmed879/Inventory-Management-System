using Abp.Application.Services.Dto;

namespace InventoryManagementSystem.ShopProducts.Dto
{
    public class PagedShopProductResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
        public double WholeSaleRate { get; set; }
        public int Quantity { get; set; }
        public double? CompanyRate { get; set; }
        public double? RetailPrice { get; set; }
        public long ProductId { get; set; }
        public long? CompanyId { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
    }
}
