using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BeePlace.Model.Expertise.Entity;
using BeePlace.Services.Expertise;
using System.Net;
using BeePlace.Model.Expertise.ValueObject;
using BeePlace.API.Standart.DTO.Expertise;

namespace BeePlace.API.Standart.Controllers.Expertise
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExpertiseController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ObjectResult> Tree()
        {
            try
            {
                List<Model.Expertise.Entity.Expertise> expertises = new List<Model.Expertise.Entity.Expertise>();
                ExpertiseService expertiseService = new ExpertiseService(Startup.BeePlaceDataBaseConnectionString);
                expertises = expertiseService.GetTree();
                return StatusCode((int)HttpStatusCode.OK, expertises);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idCnae}")]
        public ActionResult<ObjectResult> ByCnae(string idCnae)
        {
            try
            {
                List<Model.Expertise.Entity.Expertise> expertises = new List<Model.Expertise.Entity.Expertise>();
                ExpertiseService expertiseService = new ExpertiseService(Startup.BeePlaceDataBaseConnectionString);
                expertises = expertiseService.List(new CNAE() { IdCnae = idCnae });
                return StatusCode((int)HttpStatusCode.OK, expertises);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idExpertise}")]
        public ActionResult<ObjectResult> Specific(int idExpertise)
        {
            try
            {
                Model.Expertise.Entity.Expertise expertise = new Model.Expertise.Entity.Expertise();
                ExpertiseService expertiseService = new ExpertiseService(Startup.BeePlaceDataBaseConnectionString);
                expertise = expertiseService.Get(new Model.Expertise.Entity.Expertise() { IdExpertise = idExpertise });
                return StatusCode((int)HttpStatusCode.OK, expertise);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idCompany}")]
        public ActionResult<ObjectResult> ByCompany(int idCompany)
        {
            try
            {
                List<Model.Expertise.Entity.Expertise> expertises = new List<Model.Expertise.Entity.Expertise>();
                ExpertiseService expertiseService = new ExpertiseService(Startup.BeePlaceDataBaseConnectionString);
                expertises = expertiseService.ListByCompany( new Model.Profile.Company.Entity.Company() { Id = idCompany });
                return StatusCode((int)HttpStatusCode.OK, expertises);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        public ActionResult<ObjectResult> RemoveCompanyAssociation([FromBody] CompanyAssociation companyAssociation)
        {
            try
            {
                ExpertiseService expertiseService = new ExpertiseService(Startup.BeePlaceDataBaseConnectionString);
                var company   = new Model.Profile.Company.Entity.Company() { Id = companyAssociation.IdCompany };
                var expertise = new Model.Expertise.Entity.Expertise() { IdExpertise = companyAssociation.IdExpertise };
                expertiseService.RemoveCompanyAssociation(company, expertise);
                return StatusCode((int)HttpStatusCode.OK, companyAssociation);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}