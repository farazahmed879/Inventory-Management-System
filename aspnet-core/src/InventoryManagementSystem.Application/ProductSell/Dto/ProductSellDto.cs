using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.ProductSells.Dto
{
    public class ProductSellDto : FullAuditedEntity<long>
    {
        public string Status { get; set; }
        public double SellingRate { get; set; }
    }
}
