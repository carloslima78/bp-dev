using System;
using System.Net;
using BeePlace.Model.Payment.Entity;
using BeePlace.Model.Profile.Client.Entity.B2B;
using BeePlace.Services.Profile.Client;
using Microsoft.AspNetCore.Mvc;

namespace BeePlace.API.Standart.Controllers.Profile.Client
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CondominiumController : ControllerBase
    {
        [HttpGet("{cnpj}")]
        public ActionResult<ObjectResult> Validate (string cnpj)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService (Startup.BeePlaceDataBaseConnectionString);
                var condominium = condominiumService.Validate(cnpj);
                return StatusCode((int)HttpStatusCode.OK, condominium);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> InsertCondominium([FromBody] Condominium condominium)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.InsertCondominium(condominium);
                return StatusCode((int)HttpStatusCode.OK, condominium);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> UpdateCondominium([FromBody] Condominium condominium)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.UpdateCondominium(condominium);
                return StatusCode((int)HttpStatusCode.OK, condominium);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idCondominium}")]
        public ActionResult<ObjectResult> GetCondominium(int idCondominium)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                var condominium = condominiumService.GetCondominium( new Condominium() { Id = idCondominium });
                return StatusCode((int)HttpStatusCode.OK, condominium);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> InsertManager([FromBody] CondominiumManager condominiumManager)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.InsertManager(condominiumManager);
                return StatusCode((int)HttpStatusCode.OK, condominiumManager);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> UpdateManager([FromBody] CondominiumManager condominiumManager)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.UpdateManager(condominiumManager);
                return StatusCode((int)HttpStatusCode.OK, condominiumManager);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> LoginManager([FromBody] CondominiumManager condominiumManager)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumManager = condominiumService.LoginManager(condominiumManager);
                return StatusCode((int)HttpStatusCode.OK, condominiumManager);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idCondominium}")]
        public ActionResult<ObjectResult> ListManagers(int idCondominium)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                var condominiumManager = condominiumService.ListManagers( new Condominium() { Id = idCondominium });
                return StatusCode((int)HttpStatusCode.OK, condominiumManager);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> InsertTower([FromBody] CondominiumInternalTower condominiumInternalTower)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.InsertTower(condominiumInternalTower);
                return StatusCode((int)HttpStatusCode.OK, condominiumInternalTower);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> UpdateTower([FromBody] CondominiumInternalTower condominiumInternalTower)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.UpdateTower(condominiumInternalTower);
                return StatusCode((int)HttpStatusCode.OK, condominiumInternalTower);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idCondominium}")]
        public ActionResult<ObjectResult> ListTowers(int idCondominium)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                var condominiumTowers = condominiumService.ListTowers( new Condominium() { Id = idCondominium });
                return StatusCode((int)HttpStatusCode.OK, condominiumTowers);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idCondominiumTower}")]
        public ActionResult<ObjectResult> GetTower(int idCondominiumTower)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                var condominiumTower = condominiumService.GetTower(new CondominiumInternalTower() { Id = idCondominiumTower });
                return StatusCode((int)HttpStatusCode.OK, condominiumTower);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> InsertSecurityCompany([FromBody] CondominiumSecurityCompany condominiumSecurityCompany)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.InsertSecurityCompany(condominiumSecurityCompany);
                return StatusCode((int)HttpStatusCode.OK, condominiumSecurityCompany);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> UpdateSecurityCompany([FromBody] CondominiumSecurityCompany condominiumSecurityCompany)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.UpdateSecurityCompany(condominiumSecurityCompany);
                return StatusCode((int)HttpStatusCode.OK, condominiumSecurityCompany);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idCondominium}")]
        public ActionResult<ObjectResult> GetSecurityCompany(int idCondominium)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                var securityCompany = condominiumService.GetSecurityCompany(new Condominium() { Id = idCondominium });
                return StatusCode((int)HttpStatusCode.OK, securityCompany);
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
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.InsertCardData(cardData);
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
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.UpdateCardData(cardData);
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
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                condominiumService.RemoveCardData(cardData);
                return StatusCode((int)HttpStatusCode.OK, cardData);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idCondominium}")]
        public ActionResult<ObjectResult> ListCardDatas(int idCondominium)
        {
            try
            {
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                var cardDatas = condominiumService.ListCardDatas(new Condominium() { Id = idCondominium });
                return StatusCode((int)HttpStatusCode.OK, cardDatas);
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
                CondominiumService condominiumService = new CondominiumService(Startup.BeePlaceDataBaseConnectionString);
                var cardData = condominiumService.GetCardDataData(new CardData() { IdCardData = idCardData });
                return StatusCode((int)HttpStatusCode.OK, cardData);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}