using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.ProductSells.Dto
{
    public class ProductSaleGraphDto : FullAuditedEntity<long>
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }
}
