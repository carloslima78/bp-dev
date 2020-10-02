using BeePlace.Infra.DataBaseUtils;
using System.Collections.Generic;
using BeePlace.Model.Profile.Company.ValueObject;
using BeePlace.Model.Geolocation.Entity;
using System;

namespace BeePlace.Model.Profile.Company.Entity
{
    public class Company
    {
        public enum CompanyAccountStatus : int
        {
            Pendent = 1,
            Active = 2,
            Inactive = 3
        }

        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Name { get; set; }

        public string CNPJ { get; set; }

        public string PhoneArea { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public bool Available { get; set; }

        public bool Online { get; set; }

        public bool Confirmed { get; set; }

        public int IdCompanyAccountStatus { get; set; }

        [DapperIgnore]
        public Address Address { get; set; }

        [DapperIgnore]
        public CompanyCoverageArea CompanyCoverageArea { get; set; }

        //[DapperIgnore]
        //public User.Entity.User User { get; set; }

        [DapperIgnore]
        public List<Expertise.Entity.Expertise> Expertises { get; set; }

        [DapperIgnore]
        public List<CompanyPartner> CompanyPartners { get; set; }

        [DapperIgnore]
        public List<CompanyFile> CompanyFiles { get; set; }

        [DapperIgnore]
        public ReceitawsDomine ReceitawsDomine { get; set; }

        [DapperIgnore]
        public Payment.Entity.CardData CardData { get; set; }
    }
}
