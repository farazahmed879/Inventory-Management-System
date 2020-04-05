using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.ShopProducts.Dto
{
    public class ShopProductDto : FullAuditedEntity<long>
    {
        public double WholeSaleRate { get; set; }
        public int Quantity { get; set; }
        public double? CompanyRate { get; set; }
        public double? RetailPrice { get; set; }
        public string Description { get; set; }
        public long ProductId { get; set; }
        public long? CompanyId { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }

    }
}
