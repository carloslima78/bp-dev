using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Profile.Client.Entity.B2B
{
    /// <summary>
    /// Representa o síndico de um condomínio.
    /// </summary>
    public class CondominiumManager
    {
        public enum CondominiumManagerAccountStatus : int
        {
            Pendent = 1,
            Active = 2,
            Inactive = 3
        }

        public int Id{ get; set; }

        public int IdCondominium { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string CPF { get; set; }

        // Confirmou cadastro por e-mail
        public bool Confirmed { get; set; }

        public bool AcceptedTerms { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
       
        public int IdCondominiumManagerAccountStatus { get; set; }
    }
}
