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
    }
}
