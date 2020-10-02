using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;


namespace BeePlace.Model.Profile.Company.Entity
{
    /// <summary>
    /// Funcionário ou agregado da empresa.
    /// </summary>
    public class CompanyPartner
    {
        [DapperKey]
        public int Id { get; set; }

        public int IdCompany { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CPF { get; set; }

        public string RG { get; set; }

        public bool IsOwner { get; set; }

        // Confirmou cadastro por e-mail
        public bool Confirmed { get; set; }

        public bool AcceptedTerms { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool Active { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        [DapperIgnore]
        public List<CompanyPartnerFile> CompanyPartnerFiles { get; set; }
    }
}
