using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.Types.Dto
{
    public class TypeDto:FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TenantId { get; set; }
    }
}
