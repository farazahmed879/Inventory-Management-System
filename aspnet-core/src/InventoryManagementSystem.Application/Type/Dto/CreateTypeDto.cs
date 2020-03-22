using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace Planner.Types.Dto
{
    public class CreateTypeDto :Entity<long>
    {
        public string Name { get; set; }
    }
}
