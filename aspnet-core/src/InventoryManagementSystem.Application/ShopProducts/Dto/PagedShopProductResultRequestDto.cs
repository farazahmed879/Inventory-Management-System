using Abp.Application.Services.Dto;

namespace InventoryManagementSystem.ShopProducts.Dto
{
    public class PagedShopProductResultRequestDto : PagedResultRequestDto
    {
        public string ProductName { get; set; }
        public long? CompanyId { get; set; }
        public long? TypeId { get; set; }
        public long? SubTypeId { get; set; }
    }
}
