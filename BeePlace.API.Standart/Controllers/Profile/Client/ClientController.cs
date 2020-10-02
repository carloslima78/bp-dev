 using System;
using System.Net;
using BeePlace.API.Standart.DTO.Profile.Client;
using BeePlace.Model.Payment.Entity;
using BeePlace.Services.Profile.Client;
using Microsoft.AspNetCore.Mvc;

namespace BeePlace.API.Standart.Controllers.Profile.Client
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ObjectResult> Insert([FromBody] Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                client.DateCreated = DateTime.Now;
                client.DateUpdated = DateTime.Now;
                clientService.InsertClient(client);
                return StatusCode((int)HttpStatusCode.OK, client);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> Update([FromBody] Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                client.DateUpdated = DateTime.Now;
                clientService.UpdateClient(client);
                return StatusCode((int)HttpStatusCode.OK, client);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> InsertAddress([FromBody] Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                clientService.InsertAddress(client);
                return StatusCode((int)HttpStatusCode.OK, client);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> UpdateAddress([FromBody] Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                clientService.UpdateAddress(client);
                return StatusCode((int)HttpStatusCode.OK, client);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        public ActionResult<ObjectResult> RemoveAddress([FromBody] Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                clientService.DeleteAddress(client);
                return StatusCode((int)HttpStatusCode.OK, client);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> InsertCardData([FromBody] CardData  cardData)
        {
            try
            {
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                clientService.InsertCardData(cardData);
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
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                clientService.RemoveCardData(cardData);
                return StatusCode((int)HttpStatusCode.OK, cardData);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idClient}")]
        public ActionResult<ObjectResult> ListCardDatas(int idClient)
        {
            try
            {
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                var cardDatas = clientService.ListCardDatas(new Model.Profile.Client.Entity.B2C.Client() { Id = idClient });
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
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                var cardData = clientService.GetCardDataData(new CardData() { IdCardData = idCardData });
                return StatusCode((int)HttpStatusCode.OK, cardData);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> Login([FromBody] Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                client = clientService.LoginClient(client);

                if (client != null)
                {
                    return StatusCode((int)HttpStatusCode.OK, client);
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

        [HttpPut]
        public ActionResult<ObjectResult> UpdateCredentials([FromBody] ClientCredentials credentials)
        {
            try
            {
                ClientService clientService = new ClientService(Startup.BeePlaceDataBaseConnectionString);
                var client = new Model.Profile.Client.Entity.B2C.Client();
                client.Email = credentials.Email;
                client.Password = credentials.Password;
                clientService.UpdateClientCredentials(client);
                return StatusCode((int)HttpStatusCode.OK, credentials);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}