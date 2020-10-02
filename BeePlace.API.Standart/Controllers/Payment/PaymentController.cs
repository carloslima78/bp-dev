using System;
using System.Net;
using BeePlace.Services.Payment;
using BeePlace.Model.Payment.Entity;
using Microsoft.AspNetCore.Mvc;
using BeePlace.Model.Payment.ValueObject;
using System.Collections.Generic;

namespace BeePlace.API.Standart.Controllers.Payment
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ObjectResult> ListCards()
        {
            try
            {
                List<Card> cards = new List<Card>();
                PaymentService paymentService = new PaymentService(Startup.BeePlaceDataBaseConnectionString);
                cards = paymentService.ListCards();
                return StatusCode((int)HttpStatusCode.OK, cards);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> InsertPayment([FromBody] Model.Payment.Entity.Payment payment)
        {
            try
            {
                PaymentService paymentService = new PaymentService(Startup.BeePlaceDataBaseConnectionString);
                paymentService.InsertPayment(payment);
                return StatusCode((int)HttpStatusCode.OK, payment);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> UpdatePayment([FromBody] Model.Payment.Entity.Payment payment)
        {
            try
            {
                PaymentService paymentService = new PaymentService(Startup.BeePlaceDataBaseConnectionString);
                paymentService.UpdatePayment(payment);
                return StatusCode((int)HttpStatusCode.OK, payment);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> UpdateReceivable([FromBody] CompanyReceivable companyReceivable)
        {
            try
            {
                PaymentService paymentService = new PaymentService(Startup.BeePlaceDataBaseConnectionString);
                paymentService.UpdateReceivable(companyReceivable);
                return StatusCode((int)HttpStatusCode.OK, companyReceivable);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}