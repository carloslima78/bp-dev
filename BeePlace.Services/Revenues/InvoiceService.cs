using BeePlace.Infra.DataBasePersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BeePlace.Model.Revenues;

namespace BeePlace.Services.Revenues
{
    public class InvoiceService
    {
        private string Connection { get; }

        public InvoiceService(string connection)
        {
            this.Connection = connection;
        }

        public void CreateInvoice()
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    //List<Signature> signatures = new List<Signature>();
                    DateTime date = DateTime.Now.AddDays(1);
                   var signatures = standartPersistence.GetEntities<Signature>(System.Data.CommandType.Text, $"Select * from [Signature]  where IdStatus=1 and paymentdate=5 and EndDate>'{date.ToString("yyyy-MM-dd")}' and  (EndExtendPayment<'{date.ToString("yyyy-MM-dd")}' or EndExtendPayment is null)").ToList();

                    foreach (var sig in signatures)
                    {
                        Invoice invoice = new Invoice();

                        invoice.IdCompany = sig.IdCompany;
                        invoice.Amount = sig.Value;
                        invoice.IdInvoiceType = 1;
                        invoice.IdStatus = 1;
                        invoice.PaymentDate =date.Date;
                        invoice.CreatedDate = DateTime.Now;
                        standartPersistence.Insert(invoice);
                    }
                    transactionScope.Complete();
                }
                catch (Exception e)
                {
                    var txt = e.Message;
                }

            }
        }

    }
}
