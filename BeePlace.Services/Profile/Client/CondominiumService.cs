using System.Transactions;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using BeePlace.Infra.DataBasePersistence;
using System.Collections.Generic;
using BeePlace.Model.Profile.Client.Entity.B2B;
using BeePlace.Model.Geolocation.Entity;
using System.Text;
using BeePlace.Model.Geolocation.ValueObject;
using BeePlace.Infra.Integrations.ReceitaWS;
using BeePlace.Infra.Integrations.ViaCEP;
using System;
using BeePlace.Infra.Utils;
using BeePlace.Model.Payment.Entity;
using BeePlace.Model.Payment;
using BeePlace.Infra.Crypt;

namespace BeePlace.Services.Profile.Client
{
    public class CondominiumService
    {
        private string Connection { get; }

        public CondominiumService(string connection)
        {
            this.Connection = connection;
        }

        public Condominium Validate(string cnpj)
        {
            try
            {
                var condominium = new Condominium();
                ReceitaWS receitaWS = new ReceitaWS();
                ReceitaWSRoot receitaWSRoot = ReceitaWS.Validate(cnpj);
                condominium.Name = receitaWSRoot.nome.ToUpper();
                condominium.Phone = receitaWSRoot.telefone;
                condominium.CNPJ = receitaWSRoot.cnpj.Replace("/", "").Replace(".", "").Replace("-", "");

                // Localização
                ViaCEPRoot viaCEPRoot = new ViaCEPRoot();
                viaCEPRoot = ViaCEP.Validate(receitaWSRoot.cep.Replace("/", "").Replace(".", "").Replace("-", ""));

                if (viaCEPRoot != null)
                {
                    condominium.Address = new Address();
                    condominium.Address.Zip = viaCEPRoot.cep.Replace("-", "").Replace(".", "");
                    condominium.Address.Street = viaCEPRoot.logradouro;
                    condominium.Address.District = viaCEPRoot.bairro;

                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    condominium.Address.State = standartPersistence.GetEntities<State>(CommandType.Text,
                        "SELECT IdEstate, UPPER(Name) AS Name, UF FROM State WHERE UF = @UF",
                        new { UF = viaCEPRoot.uf.ToUpper() }).SingleOrDefault();

                    var city = standartPersistence.GetEntities<City>(CommandType.Text,
                        "SELECT IdCity, Name, IdEstate FROM City WHERE Name = @Name", new { Name = TextFormat.RemoveAccentuation(viaCEPRoot.localidade.ToUpper()) }).SingleOrDefault();

                    condominium.Address.State.Cities = new List<City>();
                    condominium.Address.State.Cities.Add(city);

                    condominium.Address.IdEstate = condominium.Address.State.IdEstate;
                    condominium.Address.IdCity = condominium.Address.State.Cities[0].IdCity;
                }

                return condominium;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void InsertCondominium(Condominium condominium)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    // Aceitou os termos
                    if (condominium.CondominiumManager.AcceptedTerms)
                    {
                        standartPersistence.Insert<Condominium>(condominium);
                        condominium.CondominiumManager.IdCondominium = condominium.Id;

                        standartPersistence.Insert<CondominiumManager>(condominium.CondominiumManager);

                        if (condominium.Address != null)
                        {
                            condominium.Address.DateCreated = DateTime.Now;
                            condominium.Address.DateUpdated = DateTime.Now;
                            standartPersistence.Insert<Address>(condominium.Address);

                            standartPersistence.Execute(CommandType.Text,
                                "INSERT INTO CondominiumAddress Values (@Id, @IdAddress)",
                                new
                                {
                                    Id = condominium.Id,
                                    IdAddress = condominium.Address.Id
                                });
                        }

                        transactionScope.Complete();
                    }
                    else
                    {
                        throw new System.Exception("Não aceitou os termos.");
                    }
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

        public void UpdateCondominium(Condominium condominium)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                standartPersistence.Update<Condominium>(condominium);

                if (condominium.Address != null)
                {
                    standartPersistence.Update<Address>(condominium.Address);
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public Condominium GetCondominium(Condominium condominium)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                condominium = standartPersistence.GetEntities<Condominium>(CommandType.Text,
                    "SELECT * FROM Condominium WHERE Id = @Id",
                    new { Id = condominium.Id }).SingleOrDefault();

                StringBuilder selectAddress = new StringBuilder();
                selectAddress.Append(" SELECT a.* FROM Address a ");
                selectAddress.Append(" INNER JOIN CondominiumAddress ca ON a.Id = ca.IdAddress ");
                selectAddress.Append(" INNER JOIN Condominium c ON c.Id = ca.IdCondominium ");
                selectAddress.Append(" WHERE c.Id = @IdCondominium ");
                condominium.Address = new Address();
                condominium.Address = standartPersistence.GetEntities<Address>(CommandType.Text, selectAddress.ToString(), new { IdCondominium = condominium.Id }).SingleOrDefault();

                condominium.Address.State = new State();
                condominium.Address.State = standartPersistence.GetEntities<State>(CommandType.Text,
                    "SELECT IdEstate, UPPER(Name) AS Name, UF FROM State WHERE IdEstate = @IdEstate",
                    new { IdEstate = condominium.Address.IdEstate }).SingleOrDefault();

                City city = new City();
                city = standartPersistence.GetEntities<City>(CommandType.Text,
                    "SELECT IdCity, Name, IdEstate FROM City WHERE IdCity = @IdCity",
                    new { IdCity = condominium.Address.IdCity }).SingleOrDefault();

                condominium.Address.State.Cities = new List<City>();
                condominium.Address.State.Cities.Add(city);

                return condominium;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void InsertManager(CondominiumManager condominiumManager)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                // Aceitou os termos
                if (condominiumManager.AcceptedTerms)
                {
                    condominiumManager.Password = CryptHelper.Encrypt(condominiumManager.Password);
                    standartPersistence.Insert<CondominiumManager>(condominiumManager);
                }
                else
                {
                    throw new System.Exception("Não aceitou os termos.");
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void UpdateManager(CondominiumManager condominiumManager)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                // Aceitou os termos
                if (condominiumManager.AcceptedTerms)
                {
                    condominiumManager.Password = CryptHelper.Encrypt(condominiumManager.Password);
                    standartPersistence.Update<CondominiumManager>(condominiumManager);
                }
                else
                {
                    throw new System.Exception("Não aceitou os termos.");
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public CondominiumManager LoginManager(CondominiumManager condominiumManager)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);
                string pass = CryptHelper.Encrypt(condominiumManager.Password);

                condominiumManager = standartPersistence.GetEntities<CondominiumManager>(CommandType.Text,
                    "SELECT * FROM CondominiumManager WHERE Email = @Email AND Password = @Password",
                    new
                    {
                        Email = condominiumManager.Email,
                        Password = pass

                    }).SingleOrDefault();
                return condominiumManager;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public List<CondominiumManager> ListManagers(Condominium condominium)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                var condominiumManagers = standartPersistence.GetEntities<CondominiumManager>(CommandType.Text,
                    "SELECT * FROM CondominiumManager WHERE IdCondominium = @Id",
                    new
                    {
                        Id = condominium.Id

                    }).ToList();

                return condominiumManagers;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void InsertTower(CondominiumInternalTower condominiumInternalTower)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    standartPersistence.Insert<CondominiumInternalTower>(condominiumInternalTower);

                    if (condominiumInternalTower.Id > 0)
                    {
                        for (int floor = 1; floor < condominiumInternalTower.FloorsQuantity; floor++)
                        {
                            for (int apto = 1; apto < condominiumInternalTower.ApartmentsQuantity; apto++)
                            {
                                var condominiumApartment = new CondominiumApartment();
                                condominiumApartment.IdCondominiumInternalTower = condominiumInternalTower.Id;
                                condominiumApartment.Floor = floor;
                                condominiumApartment.Number = Convert.ToInt32(floor.ToString() + apto.ToString());

                                standartPersistence.Insert<CondominiumApartment>(condominiumApartment);
                            }
                        }
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

        public void UpdateTower(CondominiumInternalTower condominiumInternalTower)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    var existCondominiumInternalTower = standartPersistence.GetEntities<CondominiumInternalTower>(CommandType.Text,
                        "SELECT * FROM CondominiumInternalTower WHERE Id = @IdTower",
                        new { IdTower = condominiumInternalTower.Id }).SingleOrDefault();

                    if (existCondominiumInternalTower.FloorsQuantity > condominiumInternalTower.FloorsQuantity)
                    {
                        int floors = existCondominiumInternalTower.FloorsQuantity - condominiumInternalTower.FloorsQuantity;

                        for (int floor = 1; floor < floors; floor++)
                        {
                            for (int apto = 1; apto < condominiumInternalTower.ApartmentsQuantity; apto++)
                            {
                                var condominiumApartment = new CondominiumApartment();
                                condominiumApartment.IdCondominiumInternalTower = condominiumInternalTower.Id;
                                condominiumApartment.Floor = floor;
                                condominiumApartment.Number = Convert.ToInt32(floor.ToString() + apto.ToString());

                                standartPersistence.Insert<CondominiumApartment>(condominiumApartment);
                            }
                        }
                    }
                    else if (existCondominiumInternalTower.FloorsQuantity < condominiumInternalTower.FloorsQuantity)
                    {
                        int floors = condominiumInternalTower.FloorsQuantity - existCondominiumInternalTower.FloorsQuantity;

                        for (int floor = 1; floor < floors; floor++)
                        {
                            for (int apto = 1; apto < condominiumInternalTower.ApartmentsQuantity; apto++)
                            {
                                var condominiumApartment = new CondominiumApartment();
                                condominiumApartment.IdCondominiumInternalTower = condominiumInternalTower.Id;
                                condominiumApartment.Floor = floor;
                                condominiumApartment.Number = Convert.ToInt32(floor.ToString() + apto.ToString());

                                standartPersistence.Delete<CondominiumApartment>(condominiumApartment);
                            }
                        }
                    }

                    standartPersistence.Update<CondominiumInternalTower>(condominiumInternalTower);

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

        public List<CondominiumInternalTower> ListTowers(Condominium condominium)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                var condominiumInternalTowers = standartPersistence.GetEntities<CondominiumInternalTower>(CommandType.Text,
                    "SELECT * FROM CondominiumInternalTower WHERE IdCondominium = @Id",
                    new { condominium.Id }).ToList();

                return condominiumInternalTowers;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public CondominiumInternalTower GetTower(CondominiumInternalTower condominiumInternalTower)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                condominiumInternalTower = standartPersistence.GetEntities<CondominiumInternalTower>(CommandType.Text,
                    "SELECT * FROM CondominiumInternalTower WHERE Id = @IdTower",
                    new { IdTower = condominiumInternalTower.Id }).SingleOrDefault();

                return condominiumInternalTower;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Listar os apartamentos por torre e andar
        /// </summary>
        /// <param name="idCondominiumInternalTower">Código da torre</param>
        /// <param name="floor">Número do andar</param>
        /// <returns>Listagem de apartamentos</returns>
        public List<CondominiumApartment> ListApartments(int idCondominiumInternalTower, int floor)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                var condominiumApartment = standartPersistence.GetEntities<CondominiumApartment>(CommandType.Text,
                    "SELECT * FROM CondominiumApartment WHERE IdCondominiumInternalTower = @IdCondominiumInternalTower AND Floor = @floor",
                    new { idCondominiumInternalTower, floor }).ToList();

                return condominiumApartment;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void InsertSecurityCompany(CondominiumSecurityCompany condominiumSecurityCompany)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                standartPersistence.Insert<CondominiumSecurityCompany>(condominiumSecurityCompany);

            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void UpdateSecurityCompany(CondominiumSecurityCompany condominiumSecurityCompany)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                standartPersistence.Update<CondominiumSecurityCompany>(condominiumSecurityCompany);

            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public CondominiumSecurityCompany GetSecurityCompany(Condominium condominium)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                var condominiumSecurityCompany = standartPersistence.GetEntities<CondominiumSecurityCompany>(CommandType.Text,
                    "SELECT * FROM CondominiumSecurityCompany WHERE IdCondominium = @Id",
                    new { Id = condominium.Id }).SingleOrDefault();

                return condominiumSecurityCompany;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void InsertCardData(CardData cardData)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                cardData.IdCardOwnerType = (int)PaymentEnums.CardOwnerType.Condominium;
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

                cardData.IdCardOwnerType = (int)PaymentEnums.CardOwnerType.Condominium;
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

        public List<CardData> ListCardDatas(Condominium condominium)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                var cardDatas = standartPersistence.GetEntities<CardData>(CommandType.Text,
                    "SELECT * FROM CardData WHERE IdCardOwner = @IdCardOwner AND IdCardOwnerType = @IdCardOwnerType ",
                    new
                    {
                        IdCardOwner = condominium.Id,
                        IdCardOwnerType = (int)PaymentEnums.CardOwnerType.Condominium
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
    }
}

