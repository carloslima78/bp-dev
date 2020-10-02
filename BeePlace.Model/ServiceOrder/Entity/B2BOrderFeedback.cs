using BeePlace.Infra.DataBaseUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class B2BOrderFeedback
    {
        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public enum Target : int { CONDOMINIUM = 1, COMPANY = 2 }

        public int IdB2BOrder { get; set; }

        public int IdUserSource { get; set; }

        public int IdUserTarget { get; set; }

        public int Points { get; set; }

        public string FeedbackDescription { get; set; }
    }
}
