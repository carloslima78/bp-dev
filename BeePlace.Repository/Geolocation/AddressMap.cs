using BeePlace.Model.Geolocation;
using BeePlace.Model.Geolocation.Entity;
using Dapper.FluentMap.Dommel.Mapping;

namespace BeePlace.Repository.Geolocation
{
    public class AddressMap : DommelEntityMap<Address>
    {
        public AddressMap()
        {
            ToTable("Address");
            Map(x => x.Id).ToColumn("IdAddress").IsIdentity().IsKey();
            Map(x => x.IdCity).ToColumn("IdCity");
            Map(x => x.IdEstate).ToColumn("IdEstate");
            Map(x => x.Zip).ToColumn("Zip");
            Map(x => x.Street).ToColumn("Street");
            Map(x => x.Number).ToColumn("Number");
            Map(x => x.Complement).ToColumn("Complement");
            Map(x => x.District).ToColumn("District");
            Map(x => x.Reference).ToColumn("Reference");
            Map(x => x.Latitude).ToColumn("Latitude");
            Map(x => x.Longitude).ToColumn("Longitude");
        }
    }
}
