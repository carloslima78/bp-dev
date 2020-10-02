using System;
using System.Net;
using BeePlace.Services.ServiceOrder;
using BeePlace.Model.ServiceOrder.Entity;
using Microsoft.AspNetCore.Mvc;
using BeePlace.Model.Profile.Company.Entity;
using BeePlace.API.Standart.DTO.ServiceOrder;

namespace BeePlace.API.Standart.Controllers.ServiceOrder
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ObjectResult> StartEstimate([FromBody] OrderEstimate order)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                orderService.StartEstimate(order);
                return StatusCode((int)HttpStatusCode.OK, order);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idestimate}")]
        public ActionResult<ObjectResult> Estimate(int idEstimate)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                var order = orderService.GetOrderEstimate(new OrderEstimate() { Id = idEstimate });
                return StatusCode((int)HttpStatusCode.OK, order);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{dateStart},{dateEnd}")]
        public ActionResult<ObjectResult> EstimateByDate(DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                var estimates = orderService.ListOrderEstimate(dateStart, dateEnd);
                return StatusCode((int)HttpStatusCode.OK, estimates);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idClient}")]
        public ActionResult<ObjectResult> EstimateByClient(int idClient)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                var estimates = orderService.ListOrderEstimate(new Model.Profile.Client.Entity.B2C.Client() { Id = idClient });
                return StatusCode((int)HttpStatusCode.OK, estimates);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idcompany}")]
        public ActionResult<ObjectResult> EstimateByCompany(int idCompany)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                var estimates = orderService.ListOrderEstimate(new Company() { Id = idCompany });
                return StatusCode((int)HttpStatusCode.OK, estimates);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idorder}")]
        public ActionResult<ObjectResult> Order(int idOrder)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                var order = orderService.GetOrder(new Order() { Id = idOrder });
                return StatusCode((int)HttpStatusCode.OK, order);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{dateStart},{dateEnd}")]
        public ActionResult<ObjectResult> OrderByDate(DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                var orders = orderService.ListOrders(dateStart, dateEnd);
                return StatusCode((int)HttpStatusCode.OK, orders);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idClient}")]
        public ActionResult<ObjectResult> OrderByClient(int idClient)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                var orders = orderService.ListOrders(new Model.Profile.Client.Entity.B2C.Client() { Id = idClient });
                return StatusCode((int)HttpStatusCode.OK, orders);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idcompany}")]
        public ActionResult<ObjectResult> OrderByCompany(int idCompany)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                var orders = orderService.ListOrders(new Company() { Id = idCompany });
                return StatusCode((int)HttpStatusCode.OK, orders);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> CancelOrderByEstimate([FromBody] CancellationByEstimate cancellation)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                Order order = new Order();
                order.Id = cancellation.IdOrder;
                order.OrderEstimate = new OrderEstimate();
                order.OrderEstimate.Id = cancellation.IdOrderEstimate;
                orderService.CancelOrderByEstimate(order);
                return StatusCode((int)HttpStatusCode.OK, cancellation);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> AcceptEstimate([FromBody] AcceptEstimateDTO acceptEstimateDTO)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                var estimate = orderService.GetOrderEstimate(new OrderEstimate() { Id = acceptEstimateDTO.IdOrderEstimate });
                if (!estimate.Accepted)
                {
                    Order order = new Order();
                    order.OrderEstimate.Accepted = acceptEstimateDTO.Accepted;
                    order.OrderEstimate.Justify = acceptEstimateDTO.Justify;
                    order.OrderEstimate.IdCompanyPartner = acceptEstimateDTO.IdCompanyPartner;
                    orderService.AcceptEstimate(order);
                    return StatusCode((int)HttpStatusCode.OK, order);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> UpdateOrderAndItems([FromBody] Order order)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                orderService.UpdateOrderAndItems(order);
                return StatusCode((int)HttpStatusCode.OK, order);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> Audit([FromBody] AuditDTO auditDTO)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                OrderItemAudit orderItemAudit = new OrderItemAudit();
                orderItemAudit.IdOrderItem = auditDTO.IdOrderItem;
                orderItemAudit.Name = auditDTO.Name;
                orderItemAudit.Type = auditDTO.Type;
                orderItemAudit.FilePath = auditDTO.FilePath;
                orderItemAudit.DateCreated = DateTime.Now;
                orderItemAudit.DateUpdated = DateTime.Now;
                orderService.CreateAudit(orderItemAudit);
                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> Close(int idOrder, int discount)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                orderService.CloseOrder(idOrder, discount);
                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> Feedback([FromBody] Order order)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                orderService.StartFeedback(order);
                return StatusCode((int)HttpStatusCode.OK, order);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> StartExpedient([FromBody] OrderExpedient expedient)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                orderService.StartExpedient(expedient);
                return StatusCode((int)HttpStatusCode.OK, expedient);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public ActionResult<ObjectResult> CloseExpedient([FromBody] OrderExpedient expedient)
        {
            try
            {
                if (expedient.Id != null && expedient.Id > 0)
                {
                    OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                    orderService.CloseExpedient(expedient);
                    return StatusCode((int)HttpStatusCode.OK, expedient);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "Id do Expediente Inválido");
                }
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ObjectResult> ScheduleByDate(ScheduleDTO scheduleDTO)
        {
            try
            {
                OrderService orderService = new OrderService(Startup.BeePlaceDataBaseConnectionString);
                var orderSchedules = orderService.ListOrderSchedule(scheduleDTO.IdCompany, scheduleDTO.DateStart, scheduleDTO.DateEnd);
                return StatusCode((int)HttpStatusCode.OK, orderSchedules);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}