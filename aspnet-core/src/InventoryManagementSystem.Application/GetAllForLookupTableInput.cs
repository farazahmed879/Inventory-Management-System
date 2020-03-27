using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagementSystem
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

    }
}
