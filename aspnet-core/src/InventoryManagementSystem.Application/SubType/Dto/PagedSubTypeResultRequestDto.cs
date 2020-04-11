using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Abp.Application.Services.Dto;

namespace InventoryManagementSystem.SubTypes.Dto
{
    public class PagedSubTypeResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
        public string ProductType { get; set; }
        public long? TenantId { get; set; }
    }
}
