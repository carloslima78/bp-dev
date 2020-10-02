using BeePlace.Infra.DataBaseUtils;
using BeePlace.Model.Expertise.ValueObject;
using System;
using System.Collections.Generic;

namespace BeePlace.Model.Expertise.Entity
{
    public class Expertise 
    {
        [DapperKey]
        public int IdExpertise { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int IdMCC { get; set; }

        public string IdCnae { get; set; }

        public int? IdFather { get; set; }

        public string Icon { get; set; }

        // Será atribuído para cada expertise que a empresa associar a ela.
        [DapperIgnore]
        public int CompanyMinCost { get; set; }

        // Será preenchido na tabela associativa de Empresa e Área para determinar se o endereço é o principal.
        [DapperIgnore]
        public bool? Main { get; set; }

        [DapperIgnore]
        public CNAE CNAE { get; set; }

        [DapperIgnore]
        public List<Expertise> Childs { get; set; }

        [DapperIgnore]
        public List<ExpertiseDetail> Details { get; set; }
    }
}
