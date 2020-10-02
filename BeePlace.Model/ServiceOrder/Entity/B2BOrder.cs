using BeePlace.Infra.DataBaseUtils;
using BeePlace.Model.Payment.Entity;
using BeePlace.Model.Profile.Client.Entity.B2B;
using BeePlace.Model.Profile.Company.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class B2BOrder
    {
        public enum Status : int
        {
            OPEN = 1,
            ClOSED = 2,
            CANCEL = 3
        }

        public int Id { get; set; }

        public int IdCondominium { get; set; }

        public int IdCondominiumManager { get; set; }

        public int IdCompany { get; set; }

        public int IdCompanyPartner { get; set; }

        public int IdAddress { get; set; }

        public int IdB2BOrderEstimate { get; set; }

        public int TotalPrice { get; set; }

        public int OrderStatus { get; set; }

        public DateTime? DateServiceStart { get; set; }

        public DateTime? DateServiceFinal { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        [DapperIgnore]
        public Condominium  Condominium { get; set; }

        [DapperIgnore]
        public Company Company { get; set; }

        [DapperIgnore]
        public B2BOrderEstimate B2BOrderEstimate { get; set; }

        [DapperIgnore]
        public List<B2BOrderItem> B2BOrderItems { get; set; }

        [DapperIgnore]
        public PaymentTransaction PaymentInvoice { get; set; }

        [DapperIgnore]
        public List<B2BOrderFeedback> B2BOrderFeedback { get; set; }
    }
}
