using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Model.Payment
{
    public class PaymentEnums
    {
        public enum CardOwnerType : int
        {
            Client = 1,
            Condominium = 2,
            Company = 3
        }

        public enum PaymentStatus : int
        {
            Pendent = 1,
            Paid = 2
        }

        public enum ReceivableStatus : int
        {
            Pendent = 1,
            Paid = 2
        }
    }
}
