using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class B2BOrderItemAudit
    {
        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public enum AuditType : int { BEFORE = 1, AFTER = 2 }

        public int IdB2BOrderItem { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public int Type { get; set; }

        [DapperIgnore]
        public string FilePath { get; set; }
    }
}
