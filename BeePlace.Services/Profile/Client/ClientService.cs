using System.Transactions;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using BeePlace.Infra.DataBasePersistence;
using System.Collections.Generic;
using System.Text;
using BeePlace.Model.Geolocation.Entity;
using BeePlace.Model.Geolocation.ValueObject;
using System;
using BeePlace.Model.Payment.Entity;
using BeePlace.Model.Payment;
using BeePlace.Infra.Crypt;

namespace BeePlace.Services.Profile.Client
{
    public class ClientService
    {
        private string Connection { get; }

        public ClientService(string connection)
        {
            this.Connection = connection;
        }

        public void InsertClient(Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                client.Password = CryptHelper.Encrypt(client.Password);
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                standartPersistence.Insert<Model.Profile.Client.Entity.B2C.Client>(client);
            }
            catch (SqlException e)
            {
                throw e;
            }

        }

        public void UpdateClient(Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                //   client.Password = CryptHelper.Encrypt(client.Password);
                //   StandartPersistence standartPersistence =
                //       new StandartPersistence(this.Connection);

                //client.DateCreated= standartPersistence.GetEntities<DateTime>(CommandType.Text, $@"select datecreated from Client  where id={client.Id}").FirstOrDefault();

                //   standartPersistence.Update(client);
                //   transactionScope.Complete();

                StandartPersistence standartPersistence =
                new StandartPersistence(this.Connection);

                standartPersistence.Execute(CommandType.Text,
                    "UPDATE Client SET FirstName = @FirstName, PhoneCodeArea = @PhoneCodeArea, Phone = @Phone, Confirmed = @Confirmed, AcceptedTerms = @AcceptedTerms, Status = @Status, Email = @Email, active = @active, DateUpdated = @DateUpdated, LastName = @LastName WHERE Id = @Id",

                    new
                    {
                        client.FirstName,
                        client.PhoneCodeArea,
                        client.Phone,
                        client.Confirmed,
                        client.AcceptedTerms,
                        client.Status,
                        client.Email,
                        client.Active,
                        client.DateUpdated,
                        client.LastName,
                        client.Id
                    });
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void InsertAddress(Model.Profile.Client.Entity.B2C.Client client)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    if (client.Address != null && client.Id > 0)
                    {
                        client.Address.DateCreated = DateTime.Now;
                        client.Address.DateUpdated = DateTime.Now;
                        standartPersistence.Insert<Address>(client.Address);

                        standartPersistence.Execute(CommandType.Text,
                            "INSERT INTO ClientAddress Values ( @IdAddress, @IdClient, @Main)",

                            new
                            {
                                IdClient = client.Id,
                                IdAddress = client.Address.Id,
                                Main = client.Address.Main
                            });
                    }

                    transactionScope.Complete();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (TransactionException e)
                {
                    throw e;
                }
            }
        }

        public void UpdateAddress(Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                if (client.Address != null)
                {
                    standartPersistence.Update<Address>(client.Address);
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void DeleteAddress(Model.Profile.Client.Entity.B2C.Client client)
        {
            StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

            if (client.Addresses != null && client.Addresses.Count > 0)
            {
                for (int i = 0; i < client.Addresses.Count; i++)
                {
                    using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        try
                        {
                            standartPersistence.Execute(CommandType.Text,
                            "DELETE FROM ClientAddress WHERE IdClient = @IdClient AND IdAddress = @IdAddress",
                            new
                            {
                                IdClient = client.Id,
                                IdAddress = client.Addresses[i].Id
                            });

                            standartPersistence.Delete<Address>(client.Addresses[i]);

                            transactionScope.Complete();
                        }
                        catch (SqlException e)
                        {
                            throw e;
                        }
                        catch (TransactionException e)
                        {
                            throw e;
                        }
                    }
                }
            }
        }

        public void InsertCardData(CardData cardData)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                cardData.IdCardOwnerType = (int)PaymentEnums.CardOwnerType.Client;
                standartPersistence.Insert<CardData>(cardData);
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void UpdateCardData(CardData cardData)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                cardData.IdCardOwnerType = (int)PaymentEnums.CardOwnerType.Client;
                standartPersistence.Update<CardData>(cardData);
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void RemoveCardData(CardData cardData)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                standartPersistence.Delete<CardData>(cardData);

            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public List<CardData> ListCardDatas(Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                var cardDatas = standartPersistence.GetEntities<CardData>(CommandType.Text,
                    "SELECT * FROM CardData WHERE IdCardOwner = @IdCardOwner AND IdCardOwnerType = @IdCardOwnerType ",
                    new
                    {
                        IdCardOwner = client.Id,
                        IdCardOwnerType = (int)PaymentEnums.CardOwnerType.Client
                    }).ToList();

                return cardDatas;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public CardData GetCardDataData(CardData cardData)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                cardData = standartPersistence.GetEntities<CardData>(CommandType.Text,
                    "SELECT * FROM CardData WHERE IdCardData = @IdCardData",
                    new { cardData.IdCardData }).SingleOrDefault();

                return cardData;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public Model.Profile.Client.Entity.B2C.Client LoginClient(Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                StringBuilder selectClient = new StringBuilder();
                string pass = CryptHelper.Encrypt(client.Password);
                selectClient.Append($"select * from Client where Email='{client.Email}' and Password='{ pass}'");
                client = standartPersistence.GetEntities<Model.Profile.Client.Entity.B2C.Client>(CommandType.Text, selectClient.ToString()).SingleOrDefault();

                StringBuilder selectAddress = new StringBuilder();
                selectAddress.Append(@" SELECT * FROM Address a
                     INNER JOIN ClientAddress ca ON a.Id = ca.IdAddress 
                     INNER JOIN Client c ON c.Id = ca.IdClient 
                     WHERE c.Id = @IdClient ");
                client.Addresses = new List<Address>();
                client.Addresses = standartPersistence.GetEntities<Address>(CommandType.Text, selectAddress.ToString(), new { IdClient = client.Id }).ToList();

                for (int i = 0; i < client.Addresses.Count; i++)
                {
                    client.Addresses[i].State = new State();
                    client.Addresses[i].State = standartPersistence.GetEntities<State>(CommandType.Text,
                    "SELECT IdEstate, UPPER(Name) AS Name, UF FROM State WHERE IdEstate = @IdEstate",
                    new { IdEstate = client.Addresses[i].IdEstate }).SingleOrDefault();

                    City city = new City();
                    city = standartPersistence.GetEntities<City>(CommandType.Text,
                    "SELECT IdCity, Name, IdEstate FROM City WHERE IdCity = @IdCity",
                    new { IdCity = client.Addresses[i].IdCity }).SingleOrDefault();

                    client.Addresses[i].State.Cities = new List<City>();
                    client.Addresses[i].State.Cities.Add(city);
                }
                return client;
            }
            catch (TransactionException e)
            {
                throw e;
            }
        }

        public void UpdateClientCredentials(Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                client.Password = CryptHelper.Encrypt(client.Password);

                standartPersistence.Execute(CommandType.Text,
                    "UPDATE Client SET Email = @Email, [Password] = @Password WHERE Email = @Email AND [Password] = @Password",
                    new { client.Email, client.Password });
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}
