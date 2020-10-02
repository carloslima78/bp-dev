using BeePlace.Model.Geolocation;
using BeePlace.Model.Geolocation.ValueObject;
using Dapper.FluentMap.Dommel.Mapping;

namespace BeePlace.Repository.Geolocation
{
    public class EstateMap : DommelEntityMap<Estate>
    {
        public EstateMap()
        {
            ToTable("Estate");
            Map(x => x.IdEstate).ToColumn("IdEstate").IsIdentity().IsKey();
            Map(x => x.Name).ToColumn("Name");
            Map(x => x.UF).ToColumn("UF");
        }
    }
}
