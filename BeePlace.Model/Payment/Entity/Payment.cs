using BeePlace.Infra.DataBaseUtils;
using System.Collections.Generic;
using System;

namespace BeePlace.Model.Payment.Entity
{
    public class Payment 
    {
        [DapperKey]
        public int IdPayment { get; set; }

        public int IdOrder { get; set; }

        public int IdCompany { get; set; }

        public int AmountTotal { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int PaymentStatus { get; set; }

        [DapperIgnore]
        public List<PaymentTransaction> PaymentTransactions  { get; set; }
    }
}
