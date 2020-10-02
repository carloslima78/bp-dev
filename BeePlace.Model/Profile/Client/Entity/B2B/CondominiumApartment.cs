using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Profile.Client.Entity.B2B
{
    public class CondominiumApartment
    {
        public int Id { get; set; }

        public int IdCondominiumInternalTower { get; set; }

        public int Floor { get; set; }

        public int Number { get; set; }
    }
}
