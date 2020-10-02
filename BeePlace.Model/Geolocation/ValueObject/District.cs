using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Geolocation.ValueObject
{
    public class District
    {
        public string StateCode { get; set; }

        public string CityName { get; set; }

        public string DistrictName { get; set; }
    }
}
