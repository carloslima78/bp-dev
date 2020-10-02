using Dapper.FluentMap.Dommel.Mapping;

namespace BeePlace.Repository.Profile.Client
{
    public class ClientMap : DommelEntityMap<Model.Profile.Client.Entity.Client>
    {
        public ClientMap()
        {
            ToTable("Client");
            Map(x => x.Id).ToColumn("IdClient").IsIdentity().IsKey();
            Map(x => x.Name).ToColumn("Name");
            Map(x => x.PhoneArea).ToColumn("PhoneArea");
            Map(x => x.PhoneNumber).ToColumn("PhoneNumber");
            Map(x => x.Email).ToColumn("Email");
        }
    }
}
