using BeePlace.Infra.DataBaseUtils;

namespace BeePlace.Model.Payment.Entity
{
    public class PaymentMethodNegotiation
    {
        [DapperKey]
        public int IdPaymentMethodNegotiation { get; set; }

        public int IdPaymentMethod { get; set; }

        public int ProviderFee { get; set; }

        public int LocalFee { get; set; }
    }
}
