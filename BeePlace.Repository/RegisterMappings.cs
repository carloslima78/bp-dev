using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using BeePlace.Repository.Geolocation;
using BeePlace.Repository.Profile.Client;
using BeePlace.Repository.Profile.User;

namespace BeePlace.Repository
{
    public class RegisterMappings
    {
        public static void Register()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new AddressMap());
                config.AddMap(new EstateMap());
                config.AddMap(new CityMap());
                config.AddMap(new ClientMap());
                config.AddMap(new UserMap());
                config.ForDommel();
            });
        }
    }
}
