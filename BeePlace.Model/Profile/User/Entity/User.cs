using BeePlace.Infra.DataBaseUtils;
using System;

namespace BeePlace.Model.Profile.User.Entity
{
    public class User 
    {
        [DapperKey]
        public int Id { get; set; }

        // Deve ser único
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
