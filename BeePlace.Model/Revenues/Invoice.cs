using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Revenues
{
    public class Invoice
    {
        public int IdInvoice { get; set; }

        public int IdCompany { get; set; }

        public int Amount { get; set; }

        public DateTime CreatedDate { get; set; }

        public int IdInvoiceType { get; set; }

        public DateTime PaymentDate { get; set; }

        public int IdStatus { get; set; }
    }
}
