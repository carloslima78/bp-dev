using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Profile.Client.Entity.B2B
{
    /// <summary>
    /// Representa a torre de um condomínio de prédios.
    /// </summary>
    public class CondominiumInternalTower
    {
        public int Id { get; set; }

        public int IdCondominium { get; set; }

        // Nome da Torre
        public string Name { get; set; }

        // Quantidade de andares
        public int FloorsQuantity { get; set; }

        // Quantidade de apartamentos
        public int ApartmentsQuantity { get; set; }
    }
}
