using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeePlace.Model.Revenues;
using BeePlace.Services.Revenues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeePlace.API.Standart.Controllers.Revenues
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        [HttpGet]
        public List<Plan> GetPlan()
        {
            PlanService planService = new PlanService(Startup.BeePlaceDataBaseConnectionString);
            List<Plan> plans = new List<Plan>();

            plans = planService.GetPlans();
            return plans;
        }

        [HttpPost]
        public Signature SetSignature(Signature signature)
        {
            PlanService planService = new PlanService(Startup.BeePlaceDataBaseConnectionString);
            planService.SetSignature(signature);
            return signature;
        }

        [HttpGet]
        public Signature GetSignature(int idCompany)
        {
            PlanService planService = new PlanService(Startup.BeePlaceDataBaseConnectionString);
            return planService.GetSignature(idCompany);
        }
    }
}