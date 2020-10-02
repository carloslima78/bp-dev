using BeePlace.Infra.DataBaseUtils;
using BeePlace.Model.Geolocation.Entity;
using BeePlace.Model.Payment.Entity;
using BeePlace.Model.Profile.Client.Entity.B2C;
using BeePlace.Model.Profile.Company.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class Order 
    {
        public enum Status: int
        {
            OPEN = 1,
            ClOSED = 2,
            CANCEL = 3
        }

        [DapperKey]
        public int Id { get; set; }

        public int IdClient { get; set; }

        public int IdCompany { get; set; }

        public int IdCompanyPartner { get; set; }

        public int IdAddress { get; set; }

        public int IdOrderEstimate { get; set; }

        public int TotalPrice { get; set; }

        public int OrderStatus { get; set; }

        public DateTime? DateServiceStart { get; set; }

        public DateTime? DateServiceFinal { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateScheduled { get; set; }

        public int HourScheduled { get; set; }

        [DapperIgnore]
        public Client Client { get; set; }

        [DapperIgnore]
        public Company Company { get; set; }

        [DapperIgnore]
        public OrderEstimate OrderEstimate { get; set; }

        [DapperIgnore]
        public OrderSchedule OrderSchedule { get; set; }

        [DapperIgnore]
        public List<OrderItem> OrderItems { get; set; }

        [DapperIgnore]
        public Payment.Entity.Payment Payment { get; set; }

        [DapperIgnore]
        public List<OrderFeedback> OrderFeedbacks { get; set; }
    }
}
