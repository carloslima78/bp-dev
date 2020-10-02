using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.DTO.ServiceOrder
{
    public class AuditDTO
    {
        public int IdOrderItem { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public string FilePath { get; set; }

    }
}
