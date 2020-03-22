using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities.Auditing;

namespace Planner.Types.Dto
{
   public class TypeDto:FullAuditedEntity<long>
    {
        public string Name { get; set; }
    }
}
