using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeePlace.API.Standart.DTO.ServiceOrder
{
    public class ScheduleDTO
    {
        public int IdCompany { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }
    }
}
