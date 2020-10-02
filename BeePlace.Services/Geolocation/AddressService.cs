using BeePlace.Infra.DataBasePersistence;
using BeePlace.Infra.Utils;
using BeePlace.Model.Geolocation;
using BeePlace.Model.Geolocation.ValueObject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using BeePlace.Model.Geolocation.Entity;

namespace BeePlace.Services.Geolocation
{
    public class AddressService
    {
        private string Connection { get; }

        public AddressService(string connection)
        {
            this.Connection = connection;
        }

        private ViaCepDomine Validate(string zip)
        {
            var viaCepDomine = new ViaCepDomine();

            try
            {
                HttpClient httpClient = new HttpClient();
                var uri = string.Format("https://viacep.com.br/ws/{0}/json", zip.Replace("-", "").Replace(".", ""));
                HttpResponseMessage result = httpClient.GetAsync(uri).Result;
                string json = result.Content.ReadAsStringAsync().Result;
                viaCepDomine = JsonConvert.DeserializeObject<ViaCepDomine>(json);
                return viaCepDomine;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Address GetValidatedAddress(string zip)
        {
            try
            {
                StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                var address = new Address();
                var viaCepDomine = this.Validate(zip);

                if (viaCepDomine != null)
                {
                    address.Zip = viaCepDomine.cep.Replace("-", "").Replace(".", "");
                    address.Street = viaCepDomine.logradouro;
                    address.District = viaCepDomine.bairro;

                    address.State = standartPersistence.GetEntities<State>(CommandType.Text,
                        "SELECT IdEstate, UPPER(Name) AS Name, UF FROM State WHERE UF = @UF",
                        new { UF = viaCepDomine.uf.ToUpper() }).SingleOrDefault();

                    var city = standartPersistence.GetEntities<City>(CommandType.Text,
                        "SELECT IdCity, Name, IdEstate FROM City WHERE IdEstate=@IdEtate and Name = @Name", new { Name = viaCepDomine.localidade.ToUpper(), IdEtate =
                    address.State.IdEstate
                        }).SingleOrDefault();

                    address.State.Cities = new List<City>();
                    address.State.Cities.Add(city);

                    address.IdEstate = address.State.IdEstate;
                    address.IdCity = address.State.Cities[0].IdCity;
                }

                return address;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Address GetEstates()
        {
            try
            {
                StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                Address address = new Address();;

                address.States = new List<State>();
                address.States = standartPersistence.GetEntities<State>(CommandType.Text,
                        "SELECT IdEstate, UPPER(Name) AS Name, UF FROM Estate",
                        null).ToList();

                return address;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Address GetCities(int idEstate)
        {
            try
            {
                StandartPersistence standartPersistence =
                       new StandartPersistence(this.Connection);

                Address address = new Address();

                address.State = new State();
                address.State = standartPersistence.GetEntities<State>(CommandType.Text,
                        "SELECT IdEstate, UPPER(Name) AS Name, UF FROM Estate WHERE IdEstate = @IdEstate",
                        new { IdEstate = idEstate }).SingleOrDefault();

                address.State.Cities = new List<City>();
                address.State.Cities = standartPersistence.GetEntities<City>(CommandType.Text,
                        "SELECT IdCity, Name, IdEstate FROM City WHERE IdEstate = @IdEstate", 
                        new { IdEstate = idEstate }).ToList();

                return address;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
