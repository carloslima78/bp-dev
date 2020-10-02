using BeePlace.Infra.DataBaseUtils;
using BeePlace.Model.Geolocation.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Profile.Client.Entity.B2B
{
    public class Condominium
    {
        public enum CondominiumType : int
        {
            Tower = 1,
            Home = 2
        }

        [DapperKey]
        public int Id { get; set; }

        public string Name { get; set; }

        public string CNPJ { get; set; }

        public string Phone { get; set; }

        public int IdCondominiumType { get; set; }

        [DapperIgnore]
        public Address Address { get; set; }

        [DapperIgnore]
        public CondominiumSecurityCompany CondominiumSecurityCompany { get; set; }

        [DapperIgnore]
        public CondominiumManager CondominiumManager { get; set; }

        [DapperIgnore]
        public List<CondominiumInternalTower> CondominiumInternalTowers { get; set; }

        [DapperIgnore]
        public List<CondominiumInternalStreet> CondominiumInternalStreets { get; set; }

        [DapperIgnore]
        public List<Payment.Entity.CardData> CardDatas { get; set; }
    }
}
