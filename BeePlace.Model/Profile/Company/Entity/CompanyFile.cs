using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Profile.Company.Entity
{
    public class CompanyFile
    {
        public enum FileType : int
        {
            // Logomarca da empresa.
            LOGO = 1, 

            // Cartão CNPJ da empresa.
            CNPJ = 2,

            // Documento diverso exemplo, algum comprovante.
            DOC = 3
        }

        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; } 

        public DateTime DateUpdated { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public int Type { get; set; }

        public int IdCompany { get; set; }

        [DapperIgnore]
        public string FilePath { get; set; }

        [DapperIgnore]
        public string Connection { get; set; }

        [DapperIgnore]
        public string Container { get; set; }
    }
}
