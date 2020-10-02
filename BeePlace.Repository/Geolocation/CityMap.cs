using BeePlace.Model.Geolocation;
using BeePlace.Model.Geolocation.ValueObject;
using Dapper.FluentMap.Dommel.Mapping;

namespace BeePlace.Repository.Geolocation
{
    public class CityMap : DommelEntityMap<City>
    {
        public CityMap()
        {
            ToTable("City");
            Map(x => x.IdCity).ToColumn("IdCity").IsIdentity().IsKey();
            Map(x => x.Name).ToColumn("Name");
            Map(x => x.IdEstate).ToColumn("IdEstate");
        }
    }
}
