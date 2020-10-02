using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Expertise.ValueObject
{
    public class CNAE
    {
        public string IdCnae { get; set; }

        public string Description { get; set; }

        public IBGEDomine IBGEDomine { get; set; }
    }
}
