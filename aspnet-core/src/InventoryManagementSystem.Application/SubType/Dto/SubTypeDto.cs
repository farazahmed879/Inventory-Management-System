using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.SubTypes.Dto
{
    public class SubTypeDto:FullAuditedEntity<long>
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public int TenantId { get; set; }
    }
}
