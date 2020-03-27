using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.SubTypes.Dto
{
    public class TypeLookupTableDto : FullAuditedEntity<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
