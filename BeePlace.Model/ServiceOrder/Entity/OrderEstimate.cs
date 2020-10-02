using BeePlace.Infra.DataBaseUtils;
using BeePlace.Model.Profile.Client.Entity.B2C;
using BeePlace.Model.Profile.Company.Entity;
using System;
using System.Collections.Generic;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class OrderEstimate 
    {
        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateScheduled { get; set; }

        public int HourScheduled { get; set; }

        public DateTime DateUpdated { get; set; }

        public int IdCompany { get; set; }

        public int IdCompanyPartner { get; set; }

        public int IdClient { get; set; }

        public int IdAddress { get; set; }

        public string ClientServiceDescription { get; set; }

        public bool Accepted { get; set; }

        public string Justify { get; set; }

        [DapperIgnore]
        public Client Client { get; set; }

        [DapperIgnore]
        public Company Company { get; set; }

        [DapperIgnore]
        public List<OrderEstimateItem> OrderEstimateItems { get; set; }
    }
}
