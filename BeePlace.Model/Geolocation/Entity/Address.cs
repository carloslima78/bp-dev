using BeePlace.Infra.DataBaseUtils;
using BeePlace.Model.Geolocation.ValueObject;
using System;
using System.Collections.Generic;

namespace BeePlace.Model.Geolocation.Entity
{
    public class Address 
    {
        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Zip { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Complement { get; set; }

        public string District { get; set; }

        public string Reference { get; set; }

        public int IdEstate { get; set; }

        public int IdCity { get; set; }

        public string Title { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        // Será preenchido na tabela associativa de Cliente e Endereço para determinar se o endereço é o principal.
        [DapperIgnore]
        public bool? Main { get; set; }

        [DapperIgnore]
        public State State { get; set; }

        [DapperIgnore]
        public List<State> States { get; set; }
    }
}
