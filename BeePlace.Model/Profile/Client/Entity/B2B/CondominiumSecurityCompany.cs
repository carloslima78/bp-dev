using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Profile.Client.Entity.B2B
{
    /// <summary>
    /// Empresa de segurança ou um contato de segurança de um condomínio com o propósito de receber o compartilhamento por e-mail
    /// das ordens de serviço.
    /// </summary>
    public class CondominiumSecurityCompany
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Contact { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        // Confirmou cadastro por e-mail
        public bool Confirmed { get; set; }

        public int IdCondominium { get; set; }

        [DapperIgnore]
        public Condominium Condominium { get; set; }
    }
}
