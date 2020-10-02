using System.Transactions;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using BeePlace.Infra.DataBasePersistence;
using System.Collections.Generic;
using BeePlace.Model.Profile.Company.ValueObject;
using BeePlace.Model.Profile.Company.Entity;
using BeePlace.Model.Geolocation.Entity;
using BeePlace.Model.Profile.User.Entity;
using System.Text;
using BeePlace.Model.Geolocation.ValueObject;
using BeePlace.Infra.AzureBlobStorage;
using System;
using BeePlace.Model.Payment.Entity;
using BeePlace.Model.Payment;
using BeePlace.Infra.Crypt;

namespace BeePlace.Services.Profile.Company
{
    public class CompanyService
    {
        private string Connection { get; }

        public CompanyService(string connection)
        {
            this.Connection = connection;
        }

        public Model.Profile.Company.Entity.Company Validate(string cnpj)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            var uri = string.Format("https://www.receitaws.com.br/v1/cnpj/{0}", cnpj);
            System.Net.Http.HttpResponseMessage result = client.GetAsync(uri).Result;
            string json = result.Content.ReadAsStringAsync().Result;
            var receitawsDomine = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceitawsDomine>(json);

            var company = new Model.Profile.Company.Entity.Company();
            company.ReceitawsDomine = new ReceitawsDomine();
            company.ReceitawsDomine = receitawsDomine;
            if (company.ReceitawsDomine.cnpj != null)
            {
                if (company.ReceitawsDomine.qsa != null && company.ReceitawsDomine.qsa.Count() > 0)
                {
                    company.CompanyPartners = new List<CompanyPartner>();

                    foreach (var socio in company.ReceitawsDomine.qsa)
                    {
                        company.CompanyPartners.Add(new CompanyPartner() { FirstName = socio.nome });
                    }
                }

                company.Name = receitawsDomine.nome;
                company.CNPJ = receitawsDomine.cnpj.Replace("/", "").Replace(".", "").Replace("-", "");
            }
            return company;
        }

        public void InsertCompany(Model.Profile.Company.Entity.Company company)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    company.DateUpdated = DateTime.Now;
                    company.DateCreated = DateTime.Now;

                    standartPersistence.Insert<Model.Profile.Company.Entity.Company>(company);

                    if (company.Address != null)
                    {
                        company.Address.DateUpdated = DateTime.Now;
                        company.Address.DateCreated = DateTime.Now;

                        standartPersistence.Insert<Address>(company.Address);

                        standartPersistence.Execute(CommandType.Text,
                            "INSERT INTO CompanyAddress Values (@IdCompany, @IdAddress)",
                            new
                            {
                                IdCompany = company.Id,
                                IdAddress = company.Address.Id
                            });
                    }

                    if (company.Expertises != null && company.Expertises.Count > 0)
                    {
                        for (int i = 0; i < company.Expertises.Count; i++)
                        {
                            standartPersistence.Execute(CommandType.Text,
                                "INSERT INTO CompanyExpertise Values (@IdCompany, @IdExpertise)",
                                new
                                {
                                    IdCompany = company.Id,
                                    IdExpertise = company.Expertises[i].IdExpertise,
                                    MinCost = company.Expertises[i].CompanyMinCost
                                });
                        }
                    }

                    if (company.CompanyPartners != null && company.CompanyPartners.Count > 0)
                    {
                        for (int i = 0; i < company.CompanyPartners.Count; i++)
                        {
                            company.CompanyPartners[i].DateUpdated = DateTime.Now;
                            company.CompanyPartners[i].DateCreated = DateTime.Now;

                            company.CompanyPartners[i].IdCompany = company.Id;
                            company.CompanyPartners[i].Password = CryptHelper.Encrypt(company.CompanyPartners[i].Password);
                            standartPersistence.Insert(company.CompanyPartners[i]);
                            if (company.CompanyPartners[i].CompanyPartnerFiles != null)
                            {
                                foreach (var file in company.CompanyPartners[i].CompanyPartnerFiles)
                                {
                                    file.DateUpdated = DateTime.Now;
                                    file.DateCreated = DateTime.Now;

                                    file.Connection = company.CompanyFiles.FirstOrDefault().Connection;
                                    file.Container = "bplace";
                                    //file.Container = file.Type == (int)Model.Profile.Company.Entity.CompanyPartnerFile.FileType.PROFILE ? "companyrecognitionfacial" : "companydocuments";


                                    var blob = AzureBlobStorage.Upload(file.FilePath,
                                         file.Name,
                                         file.Connection,
                                         file.Container
                                        );

                                    if (blob != null)
                                    {
                                        file.URL = blob.StorageUri.PrimaryUri.AbsoluteUri;
                                        file.IdCompanyPartner = company.CompanyPartners[i].Id;
                                        standartPersistence.Insert(file);
                                    }
                                    else
                                    {
                                        throw new System.Exception("Falha no processo de Upload de arquivos no AzureBlobStorage.");
                                    }
                                }
                            }
                        }
                    }

                    if (company.CompanyFiles != null && company.CompanyFiles.Count > 0)
                    {
                        for (int i = 0; i < company.CompanyFiles.Count; i++)
                        {
                            var blob = AzureBlobStorage.Upload(company.CompanyFiles[i].FilePath,
                                company.CompanyFiles[i].Name,
                                company.CompanyFiles[i].Connection,
                                company.CompanyFiles[i].Container
                                );

                            if (blob != null)
                            {
                                company.CompanyFiles[i].URL = blob.StorageUri.PrimaryUri.AbsoluteUri;
                                company.CompanyFiles[i].IdCompany = company.Id;
                                company.CompanyFiles[i].DateUpdated = DateTime.Now;
                                company.CompanyFiles[i].DateCreated = DateTime.Now;

                                standartPersistence.Insert(company.CompanyFiles[i]);
                            }
                            else
                            {
                                throw new System.Exception("Falha no processo de Upload de arquivos no AzureBlobStorage.");
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

        public void UpdateCompany(Model.Profile.Company.Entity.Company company)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);
                    company.DateUpdated = DateTime.Now;
                    standartPersistence.Update<Model.Profile.Company.Entity.Company>(company);

                    if (company.Address != null)
                    {
                        company.Address.DateUpdated = DateTime.Now;
                        standartPersistence.Update<Address>(company.Address);
                    }

                    if (company.Expertises != null && company.Expertises.Count > 0)
                    {
                        // Remove todas as associações para incluir novamente.
                        standartPersistence.Execute(CommandType.Text,
                              "DELETE CompanyExpertise WHERE @IdCompany = IdCompany",
                              new
                              {
                                  IdCompany = company.Id
                              });

                        for (int i = 0; i < company.Expertises.Count; i++)
                        {
                            standartPersistence.Execute(CommandType.Text,
                                "INSERT INTO CompanyExpertise Values (@IdCompany, @IdExpertise)",
                                new
                                {
                                    IdCompany = company.Id,
                                    IdExpertise = company.Expertises[i].IdExpertise,
                                    MinCost = company.Expertises[i].CompanyMinCost
                                });
                        }
                    }

                    if (company.CompanyPartners != null && company.CompanyPartners.Count > 0)
                    {
                        for (int i = 0; i < company.CompanyPartners.Count; i++)
                        {
                            // Caso o sócio ainda não exista, insere
                            if (company.CompanyPartners[i].Id == 0)
                            {
                                company.CompanyPartners[i].DateUpdated = DateTime.Now;
                                company.CompanyPartners[i].IdCompany = company.Id;
                                company.CompanyPartners[i].Password = CryptHelper.Encrypt(company.CompanyPartners[i].Password);
                                standartPersistence.Insert(company.CompanyPartners[i]);
                            }
                            // Caso exista, atualiza
                            else
                            {
                                company.CompanyPartners[i].DateUpdated = DateTime.Now;
                                company.CompanyPartners[i].Password = CryptHelper.Encrypt(company.CompanyPartners[i].Password);
                                standartPersistence.Update(company.CompanyPartners[i]);
                            }
                        }
                    }

                    if (company.CompanyFiles != null && company.CompanyFiles.Count > 0)
                    {
                        for (int i = 0; i < company.CompanyFiles.Count; i++)
                        {
                            if (company.CompanyFiles[i].Id == 0)
                            {
                                var blob = AzureBlobStorage.Upload(company.CompanyFiles[i].FilePath,
                                    company.CompanyFiles[i].Name,
                                    company.CompanyFiles[i].Connection,
                                    company.CompanyFiles[i].Container
                               );

                                if (blob != null)
                                {
                                    company.CompanyFiles[i].URL = blob.StorageUri.PrimaryUri.AbsoluteUri;
                                    company.CompanyFiles[i].IdCompany = company.Id;
                                    company.CompanyFiles[i].DateUpdated = DateTime.Now;
                                    standartPersistence.Insert(company.CompanyFiles[i]);
                                }
                                else
                                {
                                    throw new System.Exception("Falha no processo de Upload de arquivos no AzureBlobStorage.");
                                }
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

        public Model.Profile.Company.Entity.Company LoginCompany(CompanyPartner companyPartner)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);
                Model.Profile.Company.Entity.Company company = new Model.Profile.Company.Entity.Company();
                string pass = CryptHelper.Encrypt(companyPartner.Password);
                companyPartner = standartPersistence.GetEntities<CompanyPartner>(CommandType.Text,
                        "SELECT * FROM CompanyPartner WHERE Email = @Email AND [Password] = @Password",
                        new
                        {
                            Email = companyPartner.Email,
                            Password = pass
                        }).SingleOrDefault();

                if (companyPartner != null && companyPartner.Id > 0)
                {
                    StringBuilder selectCompany = new StringBuilder();
                    selectCompany.Append($@"select c.* from company c 
                                           join companypartner cp on cp.idcompany = c.id
                                           where cp.id= {companyPartner.Id} ");
                    company = standartPersistence.GetEntities<Model.Profile.Company.Entity.Company>(CommandType.Text, selectCompany.ToString()).SingleOrDefault();

                    company.CompanyPartners = new List<CompanyPartner>();

                    if (companyPartner.IsOwner == true)
                    {
                        string part = $@"select * from companypartner where idcompany ={company.Id}";
                        company.CompanyPartners = standartPersistence.GetEntities<CompanyPartner>(CommandType.Text, part).ToList();
                    }
                    else
                    {
                        company.CompanyPartners.Add(companyPartner);
                    }

                    StringBuilder selectAddress = new StringBuilder();
                    selectAddress.Append($@"SELECT a.* FROM Address a 
                                            INNER JOIN CompanyAddress ca ON a.Id = ca.IdAddress 
                                            INNER JOIN Company c ON c.Id = ca.IdCompany 
                                            WHERE c.Id = {company.Id}");

                    company.Address = new Address();
                    company.Address = standartPersistence.GetEntities<Address>(CommandType.Text, selectAddress.ToString()).SingleOrDefault();

                    company.Address.State = new State();
                    company.Address.State = standartPersistence.GetEntities<State>(CommandType.Text,
                        "SELECT IdEstate, UPPER(Name) AS Name, UF FROM State WHERE IdEstate = @IdEstate",
                        new { IdEstate = company.Address.IdEstate }).SingleOrDefault();

                    City city = new City();
                    city = standartPersistence.GetEntities<City>(CommandType.Text,
                        "SELECT IdCity, Name, IdEstate FROM City WHERE IdCity = @IdCity",
                        new { IdCity = company.Address.IdCity }).SingleOrDefault();

                    company.Address.State.Cities = new List<City>();
                    company.Address.State.Cities.Add(city);
                }
                else
                {
                    company = new Model.Profile.Company.Entity.Company();
                    company.CompanyPartners = new List<CompanyPartner>();
                }

                return company;
            }
            catch (TransactionException e)
            {
                throw e;
            }
        }

        public List<CompanyPartner> ListPartnersByCompany(Model.Profile.Company.Entity.Company company)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                var partners = standartPersistence.GetEntities<CompanyPartner>(CommandType.Text,
                        "SELECT * FROM CompanyPartner WHERE IdCompany = @IdCompany",
                        new { IdCompany = company.Id }).ToList();


                return partners;
            }
            catch (TransactionException e)
            {
                throw e;
            }
        }

        public void UpdateAddress(Model.Profile.Company.Entity.Company company)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    if (company.Address != null)
                    {
                        standartPersistence.Update<Address>(company.Address);
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

        public CompanyPartner InsertPartner(CompanyPartner partner)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    if (partner != null)
                    {
                        partner.Password = CryptHelper.Encrypt(partner.Password);
                        partner.DateCreated = DateTime.Now;
                        partner.DateUpdated = DateTime.Now;
                        standartPersistence.Insert(partner);
                    }

                    transactionScope.Complete();
                    partner.Password = CryptHelper.Decrypt(partner.Password);
                    return partner;
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

        public List<CompanyCoverageArea> InsertCompanyCoverageAreas(List<CompanyCoverageArea> coverageAreas)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    if (coverageAreas != null && coverageAreas.Count > 0)
                    {
                        foreach (var area in coverageAreas)
                        {
                            if (area.IdCompany > 0)
                                standartPersistence.Execute(CommandType.Text, $"INSERT INTO [CompanyCoverageArea] (IdCompany, StateCode, CityName, DistrictName) VALUES({area.IdCompany}, '{area.StateCode}', '{area.CityName}', '{area.DistrictName}')");
                        }

                    }

                    transactionScope.Complete();
                    return coverageAreas;
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

        public void InsertCardData(CardData cardData)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                cardData.IdCardOwnerType = (int)PaymentEnums.CardOwnerType.Company;
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

        public List<Model.Profile.Company.Entity.Company> GetByZip(string zip)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);
                var companies = standartPersistence.GetEntities<Model.Profile.Company.Entity.Company>(CommandType.Text,
                        $@"select c.* from Address a 
                           join CompanyAddress ca on ca.IdAddress= a.id
                           join Company c on c.id=ca.IdCompany
                           where Zip='{zip}'").ToList();

                if (companies != null && companies.Count > 0)
                {
                    foreach (var company in companies)
                    {

                        StringBuilder selectAddress = new StringBuilder();
                        selectAddress.Append($@"SELECT a.* FROM Address a 
                                            INNER JOIN CompanyAddress ca ON a.Id = ca.IdAddress 
                                            INNER JOIN Company c ON c.Id = ca.IdCompany 
                                            WHERE c.Id = {company.Id}");

                        company.Address = new Address();
                        company.Address = standartPersistence.GetEntities<Address>(CommandType.Text, selectAddress.ToString()).SingleOrDefault();

                        company.Address.State = new State();
                        company.Address.State = standartPersistence.GetEntities<State>(CommandType.Text,
                            "SELECT IdEstate, UPPER(Name) AS Name, UF FROM State WHERE IdEstate = @IdEstate",
                            new { IdEstate = company.Address.IdEstate }).SingleOrDefault();

                        City city = new City();
                        city = standartPersistence.GetEntities<City>(CommandType.Text,
                            "SELECT IdCity, Name, IdEstate FROM City WHERE IdCity = @IdCity",
                            new { IdCity = company.Address.IdCity }).SingleOrDefault();

                        company.Address.State.Cities = new List<City>();
                        company.Address.State.Cities.Add(city);

                        string x = $@"select * from CompanyExpertise ce 
                                                join Expertise e on e.IdExpertise = ce.IdExpertise
                                                where ce.IdCompany= {company.Id}";

                        company.Expertises = new List<Model.Expertise.Entity.Expertise>();
                        company.Expertises = standartPersistence.GetEntities<Model.Expertise.Entity.Expertise>(CommandType.Text, x).ToList();

                        x = $@"select * from CompanyFile
                                                where IdCompany= {company.Id}";

                        company.CompanyFiles = new List<CompanyFile>();
                        company.CompanyFiles = standartPersistence.GetEntities<CompanyFile>(CommandType.Text, x).ToList();
                    }
                }

                return companies;
            }
            catch (TransactionException e)
            {
                throw e;
            }
        }


        public List<Model.Profile.Company.Entity.Company> GetByExpertise(int idExpertise)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);
                var companies = standartPersistence.GetEntities<Model.Profile.Company.Entity.Company>(CommandType.Text,
                        $@"select c.* from expertise e
                           join companyexpertise ce on ce.idexpertise= e.IdExpertise
                           join Company c on c.id=ce.IdCompany
                           where ce.IdExpertise={idExpertise}").ToList();

                if (companies != null && companies.Count > 0)
                {
                    foreach (var company in companies)
                    {

                        StringBuilder selectAddress = new StringBuilder();
                        selectAddress.Append($@"SELECT a.* FROM Address a 
                                            INNER JOIN CompanyAddress ca ON a.Id = ca.IdAddress 
                                            INNER JOIN Company c ON c.Id = ca.IdCompany 
                                            WHERE c.Id = {company.Id}");

                        company.Address = new Address();
                        company.Address = standartPersistence.GetEntities<Address>(CommandType.Text, selectAddress.ToString()).SingleOrDefault();

                        company.Address.State = new State();
                        company.Address.State = standartPersistence.GetEntities<State>(CommandType.Text,
                            "SELECT IdEstate, UPPER(Name) AS Name, UF FROM State WHERE IdEstate = @IdEstate",
                            new { IdEstate = company.Address.IdEstate }).SingleOrDefault();

                        City city = new City();
                        city = standartPersistence.GetEntities<City>(CommandType.Text,
                            "SELECT IdCity, Name, IdEstate FROM City WHERE IdCity = @IdCity",
                            new { IdCity = company.Address.IdCity }).SingleOrDefault();

                        company.Address.State.Cities = new List<City>();
                        company.Address.State.Cities.Add(city);

                        string x = $@"select * from CompanyExpertise ce 
                                                join Expertise e on e.IdExpertise = ce.IdExpertise
                                                where ce.IdCompany= {company.Id}";

                        company.Expertises = new List<Model.Expertise.Entity.Expertise>();
                        company.Expertises = standartPersistence.GetEntities<Model.Expertise.Entity.Expertise>(CommandType.Text, x).ToList();

                        x = $@"select * from CompanyFile
                                                where IdCompany= {company.Id}";

                        company.CompanyFiles = new List<CompanyFile>();
                        company.CompanyFiles = standartPersistence.GetEntities<CompanyFile>(CommandType.Text, x).ToList();
                    }
                }

                return companies;
            }
            catch (TransactionException e)
            {
                throw e;
            }
        }

        public void UpdateCompanyPartnerCredentials(CompanyPartner companyPartner)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                companyPartner.Password = CryptHelper.Encrypt(companyPartner.Password);

                standartPersistence.Execute(CommandType.Text,
                    "UPDATE CompanyPartner SET Email = @Email, [Password] = @Password WHERE Email = @Email AND [Password] = @Password",
                    new { companyPartner.Email, companyPartner.Password });
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}
