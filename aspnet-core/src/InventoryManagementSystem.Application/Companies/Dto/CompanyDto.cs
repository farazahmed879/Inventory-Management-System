using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.Companies.Dto
{
    public class CompanyDto:FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
