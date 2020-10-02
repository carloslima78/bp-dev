
using BeePlace.Infra.DataBaseUtils;
using System;

namespace BeePlace.Model
{
    public abstract class EntityBase
    {
        [DapperKey]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
