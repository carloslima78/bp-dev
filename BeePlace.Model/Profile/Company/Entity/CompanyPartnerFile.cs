using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Profile.Company.Entity
{
    public class CompanyPartnerFile
    {
        public enum FileType : int
        {
            // Foto do funcionário.
            PROFILE = 1,

            // RG do funcionário.
            RG = 2,

            // CPF do funcionário.
            CPF = 3,

            // Documento diverso exemplo, algum comprovante.
            DOC = 4
        }

        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public int Type { get; set; }

        public int IdCompanyPartner { get; set; }

        [DapperIgnore]
        public string FilePath { get; set; }

        [DapperIgnore]
        public string Connection { get; set; }

        [DapperIgnore]
        public string Container { get; set; }
    }
}
