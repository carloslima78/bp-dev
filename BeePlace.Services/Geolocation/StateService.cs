using BeePlace.Infra.DataBasePersistence;
using BeePlace.Model.Geolocation.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Services.Geolocation
{
    public class StateService
    {
        private string Connection { get; }

        public StateService(string connection)
        {
            this.Connection = connection;
        }
       
        public List<State> GetState()
        {
            StandartPersistence standartPersistence =
                       new StandartPersistence(this.Connection);

          return standartPersistence.GetEntities<State>(System.Data.CommandType.Text,"select * from State").ToList();
        }

        public City GetCities(int IdState, string city)
        {
            StandartPersistence standartPersistence =
                       new StandartPersistence(this.Connection);

            return standartPersistence.GetEntities<City>(System.Data.CommandType.Text, $"select * from City where IdEstate ={IdState} and name like '%{city}%'").First();
        }

        public List<District> GetDistricts(string cityName, string stateCode)
        {
            StandartPersistence standartPersistence =
                      new StandartPersistence(this.Connection);

            return standartPersistence.GetEntities<District>(System.Data.CommandType.Text, $"select StateCode,CityName,DistrictName from vPostalCodes where CityName like '%{cityName}%' and StateCode='{stateCode}' group by StateCode,CityName,DistrictName").ToList();
        }

        public District GetDistrictsByZip(string zip)
        {
            StandartPersistence standartPersistence =
                      new StandartPersistence(this.Connection);

            return standartPersistence.GetEntities<District>(System.Data.CommandType.Text, $"select StateCode,CityName,DistrictName from vPostalCodes where PostalCode='{zip}' group by StateCode,CityName,DistrictName").FirstOrDefault();
        }
    }
}
