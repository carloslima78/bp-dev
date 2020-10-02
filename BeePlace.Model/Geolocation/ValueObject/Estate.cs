using BeePlace.Infra.DataBaseUtils;
using System.Collections.Generic;

namespace BeePlace.Model.Geolocation.ValueObject
{
    public class State 
    {
        [DapperKey]
        public int IdEstate { get; set; }

        public string Name { get; set; }

        public string UF { get; set; }

        [DapperIgnore]
        public List<City> Cities { get; set; }
    }
}
