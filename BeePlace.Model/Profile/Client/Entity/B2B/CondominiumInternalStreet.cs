using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Profile.Client.Entity.B2B
{
    /// <summary>
    /// Representa uma rua interna dentro de um condomínio de casas.
    /// </summary>
    public class CondominiumInternalStreet
    {
        public int IdCondominiumInternalStreet { get; set; }

        // Nome da rua
        public string Name { get; set; }

        public int IdCondominium { get; set; }
    }
}
