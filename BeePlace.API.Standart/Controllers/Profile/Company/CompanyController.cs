using System;
using System.Collections.Generic;
using System.Net;
using BeePlace.API.Standart.DTO.Profile.Company;
using BeePlace.Model.Payment.Entity;
using BeePlace.Model.Profile.Company.Entity;
using BeePlace.Services.Profile.Company;
using Microsoft.AspNetCore.Mvc;

namespace BeePlace.API.Standart.Controllers.Profile.Company
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ObjectResult> Insert([FromBody] Model.Profile.Company.Entity.Company company)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                companyService.InsertCompany(company);
                return StatusCode((int)HttpStatusCode.OK, company);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> Update([FromBody] Model.Profile.Company.Entity.Company company)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                companyService.UpdateCompany(company);
                return StatusCode((int)HttpStatusCode.OK, company);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{cnpj}")]
        public ActionResult<ObjectResult> Validate(string cnpj)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                Model.Profile.Company.Entity.Company company = new Model.Profile.Company.Entity.Company();
                company = companyService.Validate(cnpj);

                if (company.CNPJ != null)
                {
                    return StatusCode((int)HttpStatusCode.OK, company);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> Login([FromBody] CompanyPartner companyPartner)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                Model.Profile.Company.Entity.Company company = new Model.Profile.Company.Entity.Company();
                company = companyService.LoginCompany(companyPartner);

                if (company.CompanyPartners != null && company.CompanyPartners.Count>0)
                {
                    return StatusCode((int)HttpStatusCode.OK, company);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> UpdateAddress([FromBody] Model.Profile.Company.Entity.Company company)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                companyService.UpdateAddress(company);
                return StatusCode((int)HttpStatusCode.OK, company);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> InsertPartner([FromBody] CompanyPartner partner)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                companyService.InsertPartner(partner);
                return StatusCode((int)HttpStatusCode.OK, partner);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idCompany}")]
        public ActionResult<ObjectResult> PartnersByCompany(int idCompany)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                Model.Profile.Company.Entity.Company company 
                    = new Model.Profile.Company.Entity.Company() { Id = idCompany };
                var partners = new List<CompanyPartner>();
                partners = companyService.ListPartnersByCompany(company);
                return StatusCode((int)HttpStatusCode.OK, partners);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> InsertCompanyCoverageArea([FromBody] List<CompanyCoverageArea> coverageAreas)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                companyService.InsertCompanyCoverageAreas(coverageAreas);
                return StatusCode((int)HttpStatusCode.OK, coverageAreas);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> InsertCardData([FromBody] CardData cardData)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                companyService.InsertCardData(cardData);
                return StatusCode((int)HttpStatusCode.OK, cardData);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> UpdateCardData([FromBody] CardData cardData)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                companyService.UpdateCardData(cardData);
                return StatusCode((int)HttpStatusCode.OK, cardData);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        public ActionResult<ObjectResult> RemoveCardData([FromBody] CardData cardData)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                companyService.RemoveCardData(cardData);
                return StatusCode((int)HttpStatusCode.OK, cardData);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idCardData}")]
        public ActionResult<ObjectResult> GetCardDataData(int idCardData)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                var cardData = companyService.GetCardDataData(new CardData() { IdCardData = idCardData });
                return StatusCode((int)HttpStatusCode.OK, cardData);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{zip}")]
        public ActionResult<ObjectResult> GetByZip(string zip)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                var caompany = companyService.GetByZip(zip);
                return StatusCode((int)HttpStatusCode.OK, caompany);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        [HttpGet("{idExpertise}")]
        public ActionResult<ObjectResult> GetByExpertise(int idExpertise)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                var caompany = companyService.GetByExpertise(idExpertise);
                return StatusCode((int)HttpStatusCode.OK, caompany);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> UpdateCredentials([FromBody] CompanyCredentials credentials)
        {
            try
            {
                CompanyService companyService = new CompanyService(Startup.BeePlaceDataBaseConnectionString);
                var partner = new CompanyPartner();
                partner.Email = credentials.Email;
                partner.Password = credentials.Password;
                companyService.UpdateCompanyPartnerCredentials(partner);
                return StatusCode((int)HttpStatusCode.OK, credentials);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}