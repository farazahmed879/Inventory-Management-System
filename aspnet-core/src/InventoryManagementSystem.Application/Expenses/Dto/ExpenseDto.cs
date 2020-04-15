using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Expenses.Dto
{
    public class ExpenseDto : FullAuditedEntity<long>
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }

        public int TenantId { get; set; }
    }
}
