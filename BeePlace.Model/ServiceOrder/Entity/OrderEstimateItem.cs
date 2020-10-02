using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class OrderEstimateItem
    {
        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int IdOrderEstimate { get; set; }

        public int UnitPrice { get; set; }

        public int IdExpertise { get; set; }

        [DapperIgnore]
        public List<OrderItemAudit> Audits { get; set; }

        [DapperIgnore]
        public Expertise.Entity.Expertise Expertise { get; set; }
    }
}