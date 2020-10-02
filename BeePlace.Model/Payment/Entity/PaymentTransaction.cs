using BeePlace.Infra.DataBaseUtils;
using System;


namespace BeePlace.Model.Payment.Entity
{
    public class PaymentTransaction
    {

        [DapperKey]
        public int IdPaymentTransaction { get; set; }

        public int IdPayment { get; set; }

        public int IdPaymentMethod { get; set; }

        public int IdCompany { get; set; }

        public int Installments { get; set; }

        public int Amount { get; set; }

        public int IdPaymentData { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int PaymentStatus { get; set; }
    }
}
