using BeePlace.Infra.DataBaseUtils;
using BeePlace.Model.Profile.Client.Entity.B2B;
using BeePlace.Model.Profile.Company.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class B2BOrderEstimate
    {
        [DapperKey]
        public int Id { get; set; }

        public int IdCompany { get; set; }

        public int IdCondominium { get; set; }

        public int IdCondominiumManager { get; set; }

        public int IdAddress { get; set; }

        public string CondominiumServiceDescription { get; set; }

        public bool Accepted { get; set; }

        public string Justify { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        [DapperIgnore]
        public Condominium Condominium  { get; set; }

        [DapperIgnore]
        public Company Company { get; set; }

        [DapperIgnore]
        public List<B2BOrderEstimateItem> B2BOrderEstimateItems { get; set; }
       
    }
}
