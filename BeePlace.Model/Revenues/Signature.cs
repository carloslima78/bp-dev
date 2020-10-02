using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Revenues
{
    public class Signature
    {
        public int Id { get; set; }

        public int IdPlan { get; set; }

        public int IdCompany { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? StartExtendPayment { get; set; }

        public DateTime? EndExtendPayment { get; set; }

        public int PaymentDate { get; set; }

        public int Value { get; set; }

        public int IdStatus { get; set; }
    }
}
