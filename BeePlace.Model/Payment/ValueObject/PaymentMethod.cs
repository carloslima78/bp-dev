
using BeePlace.Infra.DataBaseUtils;

namespace BeePlace.Model.Payment.ValueObject
{
    public class PaymentMethod
    {
        [DapperKey]
        public int IdPaymentMethod { get; set; }

        public string Name { get; set; }

        public int IdCard { get; set; }

        public string Image { get; set; }
    }
}
