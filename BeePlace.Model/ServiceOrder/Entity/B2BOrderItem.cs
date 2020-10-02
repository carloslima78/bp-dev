using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class B2BOrderItem
    {
        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int IdB2BOrder { get; set; }

        public int UnitPrice { get; set; }

        public int IdExpertise { get; set; }

        public int OrderItemStatus { get; set; }

        public string CondominiumJustity { get; set; }

        public string CompanyJustity { get; set; }

        [DapperIgnore]
        public Expertise.Entity.Expertise Expertise { get; set; }

        [DapperIgnore]
        public List<B2BOrderItemAudit> B2BOrderItemAudits { get; set; }
    }
}
