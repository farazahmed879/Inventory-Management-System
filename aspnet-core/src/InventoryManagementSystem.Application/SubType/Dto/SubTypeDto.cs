using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.SubTypes.Dto
{
    public class SubTypeDto:FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string ProductTypeName { get; set; }
        public long ProductTypeId { get; set; }
    }
}
