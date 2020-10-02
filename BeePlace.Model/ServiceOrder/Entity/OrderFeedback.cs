

using BeePlace.Infra.DataBaseUtils;
using System;

namespace BeePlace.Model.ServiceOrder.Entity
{
    public class OrderFeedback
    {
        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public enum Target : int { CLIENT = 1, COMPANY = 2 }

        public int IdOrder { get; set; }

        public int IdUserSource { get; set; }

        public int IdUserTarget { get; set; }

        public int Points { get; set; }

        public string FeedbackDescription { get; set; }
    }
}
