using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Abp.Application.Services.Dto;

namespace InventoryManagementSystem.Products.Dto
{
    public class PagedProductResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
        public long? SubTypeId { get; set; }
    }
}
