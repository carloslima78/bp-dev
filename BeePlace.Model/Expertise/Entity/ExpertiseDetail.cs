using BeePlace.Infra.DataBaseUtils;
using System.Collections.Generic;

namespace BeePlace.Model.Expertise.Entity
{
    public class ExpertiseDetail
    {
        [DapperKey]
        public int IdExpertiseDetail { get; set; }

        public int IdExpertise { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int MinPrice { get; set; }

        public string Image { get; set; }
    }
}
