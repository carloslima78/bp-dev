using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class OrderExpedient
    {
        [DapperKey]
        public int Id { get; set; }

        public int IdOrder { get; set; }

        public int IdCompanyPartner { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Comment { get; set; }

        public int IdStatus { get; set; }
    }
}
