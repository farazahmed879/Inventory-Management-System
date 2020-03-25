using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.ShopProducts.Dto
{
    public class ShopProductDto : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public double WholeSaleRate { get; set; }
        public int Quantity { get; set; }
        public double? CompanyRate { get; set; }
        public double? RetailPrice { get; set; }

    }
}
