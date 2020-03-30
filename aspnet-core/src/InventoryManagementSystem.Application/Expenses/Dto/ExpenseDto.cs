using Abp.Domain.Entities.Auditing;

namespace InventoryManagementSystem.Expenses.Dto
{
    public class ExpenseDto : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
    }
}
