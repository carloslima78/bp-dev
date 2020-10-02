
using BeePlace.Infra.DataBaseUtils;

namespace BeePlace.Model.Payment.ValueObject
{
    public class Card
    {
        [DapperKey]
        public int IdCard { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }
    }
}
