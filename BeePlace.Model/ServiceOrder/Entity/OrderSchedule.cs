using BeePlace.Infra.DataBaseUtils;
using System;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class OrderSchedule
    {
        [DapperKey]
        public int Id { get; set; }

        public int IdCompany { get; set; }

        public int IdOrder { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateScheduled { get; set; }

        public int HourScheduled { get; set; }
    }
}
