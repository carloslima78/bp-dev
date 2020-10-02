using BeePlace.Infra.DataBasePersistence;
using BeePlace.Model.Revenues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeePlace.Services.Revenues
{
    public class PlanService
    {
        private string Connection { get; }

        public PlanService(string connection)
        {
            this.Connection = connection;
        }


        public List<Plan> GetPlans()
        {
            StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

            List<Plan> plans = new List<Plan>();

            plans = standartPersistence.GetEntities<Plan>(System.Data.CommandType.Text, "select * from [plan] where idstatus=1").ToList();

            return plans;
        }

        public Signature SetSignature(Signature signature)
        {
            StandartPersistence standartPersistence =
                                   new StandartPersistence(this.Connection);
            var x = standartPersistence.GetEntities<Signature>(System.Data.CommandType.Text, $"Select * from [signature] where idcompany={signature.IdCompany}").FirstOrDefault();
            var p = standartPersistence.GetEntities<Plan>(System.Data.CommandType.Text, $"Select * from [Plan] where id={signature.IdPlan}").FirstOrDefault();

            signature.IdStatus = 1;
            signature.StartDate = DateTime.Now;
            signature.EndDate = DateTime.Now.AddMonths(p.Period);
            signature.Value = p.Value;
            signature.IdStatus = 1;
            if (x != null && x.Id > 0)
            {

                x.IdPlan = signature.IdPlan;
                x.IdStatus = signature.IdPlan;
                x.PaymentDate = signature.PaymentDate;
                x.StartDate = signature.StartDate;
                x.StartExtendPayment = signature.StartExtendPayment;
                x.Value = signature.Value;
                x.EndDate = signature.EndDate;
                x.EndExtendPayment = signature.EndExtendPayment;
                standartPersistence.Update(x);
                return x;
            }
            else
            {
                standartPersistence.Insert(signature);
                return signature;
            }

        }

        public Signature GetSignature(int idCompany)
        {
            StandartPersistence standartPersistence =
                                 new StandartPersistence(this.Connection);
            return  standartPersistence.GetEntities<Signature>(System.Data.CommandType.Text, $"Select * from [signature] where idcompany={idCompany}").FirstOrDefault();
        }
    }
}
