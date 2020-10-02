using BeePlace.Infra.DataBaseUtils;
using BeePlace.Model.Geolocation;
using BeePlace.Model.Geolocation.Entity;
using BeePlace.Model.Profile;
using System;
using System.Collections.Generic;

namespace BeePlace.Model.Profile.Client.Entity.B2C
{
    public class Client 
    {
        public enum ClientAccountStatus : int
        {
            Pendent = 1,
            Active = 2,
            Inactive = 3
        }

        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneCodeArea { get; set; }

        public string Phone { get; set; }

        public bool Confirmed { get; set; }

        public bool AcceptedTerms { get; set; }

        public int Status { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool Active { get; set; }

        [DapperIgnore]
        public List<Address> Addresses { get; set; }

        [DapperIgnore]
        public Address Address { get; set; }

        [DapperIgnore]
        public List<Payment.Entity.CardData> CardDatas { get; set; }
    }
}
