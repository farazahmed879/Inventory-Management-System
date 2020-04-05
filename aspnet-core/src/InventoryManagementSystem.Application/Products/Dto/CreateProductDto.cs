using Abp.Domain.Entities;

namespace InventoryManagementSystem.Products.Dto
{
    public class CreateProductDto : Entity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long SubTypeId { get; set; }
    }
}
