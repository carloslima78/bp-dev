using BeePlace.Infra.DataBasePersistence;
using BeePlace.Model.Payment.Entity;
using BeePlace.Model.Payment.ValueObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BeePlace.Model.Payment;

namespace BeePlace.Services.Payment
{
    public class PaymentService
    {
        private string Connection { get; }

        public PaymentService(string connection)
        {
            this.Connection = connection;
        }

        public void InsertPayment(Model.Payment.Entity.Payment payment)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    if (payment.PaymentTransactions.Count > 0)
                    {
                        payment.DateCreated = DateTime.Now;
                        payment.DateUpdated = DateTime.Now;
                        payment.PaymentStatus = (int)PaymentEnums.PaymentStatus.Pendent;
                        payment.AmountTotal = payment.PaymentTransactions.Sum(p => p.Amount);
                        standartPersistence.Insert<Model.Payment.Entity.Payment>(payment);

                        // Caso o valor total do pagamento for diferente da soma dos valores presentes nos pagamentos unitários, deve retornar erro.
                        if (payment.AmountTotal != payment.PaymentTransactions.Sum(x => x.Amount))
                        {
                            throw new Exception("Valores total do pagamento e soma dos pagamentos unitários não coincidem.");
                        }
                        else
                        {
                            for (int i = 0; i < payment.PaymentTransactions.Count; i++)
                            {
                                payment.PaymentTransactions[i].PaymentStatus = (int)PaymentEnums.PaymentStatus.Pendent;
                                payment.PaymentTransactions[i].IdPaymentTransaction = payment.IdPayment;
                                payment.PaymentTransactions[i].IdCompany = payment.IdCompany;
                                payment.PaymentTransactions[i].DateCreated = DateTime.Now;
                                payment.PaymentTransactions[i].DateUpdated = DateTime.Now;
                               standartPersistence.Insert<PaymentTransaction>(payment.PaymentTransactions[i]);

                                var negotiation = standartPersistence.GetEntities<PaymentMethodNegotiation>(System.Data.CommandType.Text,
                                   $"SELECT * FROM PaymentMethodNegotiation WHERE IdPaymentMethod = {payment.PaymentTransactions[i].IdPaymentMethod}").SingleOrDefault();

                                double amount = (double)payment.PaymentTransactions[i].Amount;
                                double localFee = (double)negotiation.LocalFee;
                                double providerFee = (double)negotiation.ProviderFee;
                                double localSpread = ((localFee * amount) / 100) / 100;
                                double providerSpread = ((providerFee * amount) / 100) / 100;
                                double discount = localSpread + providerSpread;
                                double companyNet = amount - discount;

                                // Verifica se existe diferença na divisão das parcelas, caso exista, a diferença será somada na última parcela
                                int result = 0;
                                int difference = 0;
                                if (payment.PaymentTransactions[i].Installments > 1)
                                {
                                    result = (int)companyNet / payment.PaymentTransactions[i].Installments;
                                    difference = (int)companyNet - (payment.PaymentTransactions[i].Installments * result);
                                }

                                for (int installment = 0; installment < payment.PaymentTransactions[i].Installments; installment++)
                                {                                  
                                    CompanyReceivable companyReceivable = new CompanyReceivable();
                                    companyReceivable.IdPaymentTransaction = payment.PaymentTransactions[i].IdPaymentTransaction;
                                    companyReceivable.IdCompany = payment.PaymentTransactions[i].IdCompany;
                                    companyReceivable.Installment = i + 1;
                                    companyReceivable.LocalFee = negotiation.LocalFee;
                                    companyReceivable.ProviderFee = negotiation.ProviderFee;
                                    companyReceivable.Amount = (int)Math.Round(amount);
                                    companyReceivable.LocalSpread = (int)Math.Round(localSpread);
                                    companyReceivable.ProviderSpread = (int)Math.Round(providerSpread);
                                    companyReceivable.Discount = (int)Math.Round(discount);
                                    companyReceivable.CompanyNet = (int)Math.Round(companyNet);
                                    companyReceivable.DateCreated = DateTime.Now;
                                    companyReceivable.DateUpdated = DateTime.Now;
                                    companyReceivable.DateEstimatePayment = DateTime.Now.AddDays(2);
                                    companyReceivable.IdReceivalbeStatus = (int)PaymentEnums.ReceivableStatus.Pendent;

                                    // Atribui a diferença entre as parcelas na última parcela.
                                    if (payment.PaymentTransactions[i].Installments > 1 && payment.PaymentTransactions[i].Installments == installment)
                                    {
                                        companyReceivable.CompanyNet += difference;
                                    }

                                    standartPersistence.Insert<CompanyReceivable>(companyReceivable);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Não existem transações de pagamento.");
                    }

                    transactionScope.Complete();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (TransactionException e)
                {
                    throw e;
                }
            }
        }

        public void UpdatePayment(Model.Payment.Entity.Payment payment)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    if (payment.PaymentTransactions.Count > 0)
                    {
                        payment.DateUpdated = DateTime.Now;
                        standartPersistence.Update<Model.Payment.Entity.Payment>(payment);

                        for (int i = 0; i < payment.PaymentTransactions.Count; i++)
                        {
                            payment.PaymentTransactions[i].DateUpdated=DateTime.Now;
                            standartPersistence.Update<PaymentTransaction>(payment.PaymentTransactions[i]);
                        }
                    }
                    else
                    {
                        throw new Exception("Não existem transações de pagamento.");
                    }

                    transactionScope.Complete();
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
        }

        public void UpdateReceivable(CompanyReceivable companyReceivable)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                standartPersistence.Update<CompanyReceivable>(companyReceivable);

            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public List<Card> ListCards()
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                List<Card> cards = new List<Card>();
                cards = standartPersistence.GetEntities<Card>(System.Data.CommandType.Text, "SELECT * FROM Card", null).ToList();
                return cards;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}
