using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Abp.Application.Services.Dto;

namespace InventoryManagementSystem.Types.Dto
{
    public class PagedTypeResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
        public long? TenantId { get; set; }
    }
}
