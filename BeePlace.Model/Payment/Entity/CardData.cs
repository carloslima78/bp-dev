using BeePlace.Infra.DataBaseUtils;
using System.Collections.Generic;
using System;

namespace BeePlace.Model.Payment.Entity
{
    public class CardData
    {
        [DapperKey]
        public int IdCardData { get; set; }

        // Enumera o código do proprietário do cartão, podendo ser o administrador do condomínio ou o cliente.
        public int IdCardOwner { get; set; }

        public int IdCardOwnerType { get; set; }

        public int IdCard { get; set; }

        public string CardOwnerDocument { get; set; }

        public string CardOwnerName { get; set; }

        public string CardNumber { get; set; }

        public string CardCVV { get; set; }

        public int CardMonth { get; set; }

        public int CardYear { get; set; }
    }
}
