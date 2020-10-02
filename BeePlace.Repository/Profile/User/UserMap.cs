using Dapper.FluentMap.Dommel.Mapping;

namespace BeePlace.Repository.Profile.User
{
    public class UserMap : DommelEntityMap<Model.Profile.User.Entity.User>
    {
        public UserMap()
        {
            ToTable("User");
            Map(x => x.Id).ToColumn("IdUser").IsIdentity().IsKey();
            Map(x => x.Email).ToColumn("Email");
            Map(x => x.Password).ToColumn("Password");
            Map(x => x.IdStatus).ToColumn("IdStatus");
        }
    }
}
