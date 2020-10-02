using BeePlace.Infra.DataBaseUtils;
using BeePlace.Model.Expertise.Entity;
using BeePlace.Model.Profile.Company.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class OrderItem
    {
        public enum Status : int
        {
            PENDENTE = 1,
            OK = 2,
            CANCELADO = 3
        }

        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int IdOrder { get; set; }

        public int UnitPrice { get; set; }

        public int IdExpertise { get; set; }

        public int OrderItemStatus { get; set; }

        public string ClientJustity { get; set; }

        public string CompanyJustity { get; set; }

        [DapperIgnore]
        public Expertise.Entity.Expertise Expertise { get; set; }

        [DapperIgnore]
        public List<OrderItemAudit> OrderItemAudits { get; set; }
    }
}
