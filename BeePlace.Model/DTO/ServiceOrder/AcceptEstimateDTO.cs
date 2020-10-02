using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.DTO.ServiceOrder
{
    public class AcceptEstimateDTO
    {
        public int IdOrderEstimate { get; set; }

        public int IdCompanyPartner { get; set; }

        public bool Accepted { get; set; }

        public string Justify { get; set; }
    }
}
