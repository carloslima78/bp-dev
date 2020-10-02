﻿using System.Collections.Generic;

namespace BeePlace.Model.Profile.Company.ValueObject
{
    public class ReceitawsDomine
    {
        public string data_situacao { get; set; }

        public string complemento { get; set; }

        public string nome { get; set; }

        public string cnpj { get; set; }

        public string uf { get; set; }

        public string telefone { get; set; }

        public string cep { get; set; }

        public List<ReceitawsAtividade> atividade_principal { get; set; }

        public List<ReceitawsAtividade> atividades_secundarias { get; set; }

        public List<ReceitawsQuadroSocietario> qsa { get; set; }
    }

    public class ReceitawsAtividade
    {
        public string text { get; set; }

        public string code { get; set; }
    }

    public class ReceitawsQuadroSocietario
    {
        public string qual { get; set; }

        public string nome { get; set; }

        public string nome_rep_legal { get; set; }

        public string qual_rep_legal { get; set; }
    }
}
