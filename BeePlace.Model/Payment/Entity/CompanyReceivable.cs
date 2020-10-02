using BeePlace.Infra.DataBaseUtils;
using System;

namespace BeePlace.Model.Payment.Entity
{
    public class CompanyReceivable
    {
        [DapperKey]
        public int IdCompanyReceivable { get; set; }

        public int IdPaymentTransaction { get; set; }

        public int IdCompany { get; set; }

        public int Installment { get; set; }

        public int ProviderFee { get; set; }

        public int LocalFee { get; set; }

        public int Amount { get; set; }

        public int ProviderSpread { get; set; }

        public int LocalSpread { get; set; }

        public int Discount { get; set; }

        public int CompanyNet { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateEstimatePayment { get; set; }

        public int IdReceivalbeStatus { get; set; }
    }
}
