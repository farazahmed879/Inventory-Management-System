using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.ProductSales.Dto
{
    public class ProductSaleDto : FullAuditedEntity<long>
    {
        public string Status { get; set; }
        public double SellingRate { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public string ProductType { get; set; }
        public double Profit { get; set; }
        public long ShopProductId { get; set; }
        public long? Quantity { get; set; }
    }
}
