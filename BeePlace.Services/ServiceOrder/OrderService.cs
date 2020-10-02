using System.Transactions;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using BeePlace.Infra.DataBasePersistence;
using System.Collections.Generic;
using BeePlace.Model.ServiceOrder.Entity;
using BeePlace.Model.Profile.Company.Entity;
using BeePlace.Model.Geolocation.Entity;
using BeePlace.Infra.AzureBlobStorage;
using System;

namespace BeePlace.Services.ServiceOrder
{
    public class OrderService
    {
        private string Connection { get; }

        public OrderService(string connection)
        {
            this.Connection = connection;
        }

        /// <summary>
        /// Inicializa uma solicitação de orçamento de prestação de serviço.
        /// </summary>
        /// <param name="order"></param>
        public void StartEstimate(OrderEstimate order)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);
                    order.DateCreated = DateTime.Now;
                    order.DateUpdated = DateTime.Now;
                    standartPersistence.Insert<OrderEstimate>(order);

                    if (order.OrderEstimateItems.Count > 0)
                    {
                        for (int i = 0; i < order.OrderEstimateItems.Count; i++)
                        {
                            order.OrderEstimateItems[i].IdOrderEstimate = order.Id;
                            order.OrderEstimateItems[i].DateCreated = DateTime.Now;
                            order.OrderEstimateItems[i].DateUpdated = DateTime.Now;
                            standartPersistence.Insert<OrderEstimateItem>(order.OrderEstimateItems[i]);
                            if (order.OrderEstimateItems[i].Audits != null && order.OrderEstimateItems[i].Audits.Count > 0)
                            {
                                foreach (var image in order.OrderEstimateItems[i].Audits)
                                {
                                    image.IdOrderItem = order.OrderEstimateItems[i].Id;
                                    CreateAudit(image);
                                }
                            }
                        }
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

        /// <summary>
        /// Recupera um orçamento pelo código da empresa associada.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public List<OrderEstimate> ListOrderEstimate(Company company)
        {
            try
            {
                StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                List<OrderEstimate> estimates = new List<OrderEstimate>();

                estimates = standartPersistence.GetEntities<OrderEstimate>(
                    CommandType.Text,
                    "SELECT * FROM OrderEstimate WHERE IdCompany = @IdCompany",
                    new { IdCompany = company.Id }).ToList();

                foreach (var estimate in estimates)
                {
                    estimate.Company = new Company();
                    estimate.Company = standartPersistence.GetEntities<Company>(
                        CommandType.Text,
                        "SELECT * FROM Company WHERE Id = @IdCompany",
                        new { IdCompany = company.Id }).SingleOrDefault();

                    estimate.OrderEstimateItems = new List<OrderEstimateItem>();
                    estimate.OrderEstimateItems = standartPersistence.GetEntities<OrderEstimateItem>(
                        CommandType.Text,
                        "SELECT * FROM OrderEstimateItem WHERE IdOrderEstimate = @IdOrderEstimate",
                        new { IdOrderEstimate = estimate.Id }).ToList();

                    estimate.Client = new Model.Profile.Client.Entity.B2C.Client();
                    estimate.Client = standartPersistence.GetEntities<Model.Profile.Client.Entity.B2C.Client>(
                        CommandType.Text,
                        "SELECT * FROM Client WHERE Id = @IdClient",
                        new { IdClient = estimate.IdClient }).SingleOrDefault();

                    estimate.Client.Address = new Address();
                    estimate.Client.Address = standartPersistence.GetEntities<Address>(
                        CommandType.Text,
                        "SELECT * FROM Address WHERE Id = @IdAddress",
                        new { estimate.IdAddress }).SingleOrDefault();
                }

                return estimates;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Recupera um orçamento pelo código do orçamento para que seja visualizado pelo cliente caso negado para gerar uma nova solicitação do mesmo orçamento para outra empresa.
        /// </summary>
        /// <param name="orderEstimate"></param>
        /// <returns></returns>
        public OrderEstimate GetOrderEstimate(OrderEstimate orderEstimate)
        {
            try
            {
                StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                OrderEstimate estimate = new OrderEstimate();

                estimate = standartPersistence.GetEntities<OrderEstimate>(
                    CommandType.Text,
                    "SELECT * FROM OrderEstimate WHERE Id = @IdOrderEstimate",
                    new { IdOrderEstimate = estimate.Id }).SingleOrDefault();

                estimate.Company = new Company();
                estimate.Company = standartPersistence.GetEntities<Company>(
                    CommandType.Text,
                    "SELECT * FROM Company WHERE Id = @IdCompany",
                    new { IdCompany = estimate.Id }).SingleOrDefault();

                estimate.OrderEstimateItems = new List<OrderEstimateItem>();
                estimate.OrderEstimateItems = standartPersistence.GetEntities<OrderEstimateItem>(
                     CommandType.Text,
                     "SELECT * FROM OrderEstimateItem WHERE IdOrderEstimate = @IdOrderEstimate",
                     new { IdOrderEstimate = estimate.Id }).ToList();

                estimate.Client = new Model.Profile.Client.Entity.B2C.Client();
                estimate.Client = standartPersistence.GetEntities<Model.Profile.Client.Entity.B2C.Client>(
                    CommandType.Text,
                    "SELECT * FROM Client WHERE Id = @IdClient",
                    new { IdClient = estimate.IdClient }).SingleOrDefault();

                estimate.Client.Address = new Address();
                estimate.Client.Address = standartPersistence.GetEntities<Address>(
                    CommandType.Text,
                    "SELECT * FROM Address WHERE Id = @IdAddress",
                    new { estimate.IdAddress }).SingleOrDefault();

                return estimate;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Order GetOrder(Order order)
        {
            try
            {
                StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);


                int idOrder = order.Id;
                order = new Order();
                order = standartPersistence.GetEntities<Order>(
                    CommandType.Text,
                    "SELECT * FROM [Order] WHERE Id = @IdOrder",
                    new { IdOrder = order.Id }).SingleOrDefault();

                order.Company = new Company();
                order.Company = standartPersistence.GetEntities<Company>(
                    CommandType.Text,
                    "SELECT * FROM Company WHERE Id = @IdCompany",
                    new { IdCompany = order.OrderEstimate.Id }).SingleOrDefault();

                order.OrderItems = new List<OrderItem>();
                order.OrderItems = standartPersistence.GetEntities<OrderItem>(
                    CommandType.Text,
                    "SELECT * FROM OrderItem WHERE IdOrder = @IdOrder",
                    new { IdOrder = order.Id }).ToList();

                order.Client = new Model.Profile.Client.Entity.B2C.Client();
                order.Client = standartPersistence.GetEntities<Model.Profile.Client.Entity.B2C.Client>(
                    CommandType.Text,
                    "SELECT * FROM Client WHERE Id = @IdClient",
                    new { IdClient = order.IdClient }).SingleOrDefault();

                order.Client.Address = new Address();
                order.Client.Address = standartPersistence.GetEntities<Address>(
                    CommandType.Text,
                    "SELECT * FROM Address WHERE Id = @IdAddress",
                    new { order.IdAddress }).SingleOrDefault();

                return order;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public List<Order> ListOrders(Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                List<Order> orders = new List<Order>();

                orders = standartPersistence.GetEntities<Order>(
                    CommandType.Text,
                    "SELECT * FROM Order WHERE IdClient = @IdClient",
                    new { IdClient = client.Id }).ToList();

                foreach (var order in orders)
                {
                    order.Company = new Company();
                    order.Company = standartPersistence.GetEntities<Company>(
                        CommandType.Text,
                        "SELECT * FROM Company WHERE Id = @IdCompany",
                        new { IdCompany = order.IdCompany }).SingleOrDefault();

                    order.OrderItems = new List<OrderItem>();
                    order.OrderItems = standartPersistence.GetEntities<OrderItem>(
                        CommandType.Text,
                        "SELECT * FROM OrderItem WHERE IdOrder = @IdOrder",
                        new { IdOrder = order.Id }).ToList();

                    order.Client = new Model.Profile.Client.Entity.B2C.Client();
                    order.Client = standartPersistence.GetEntities<Model.Profile.Client.Entity.B2C.Client>(
                        CommandType.Text,
                        "SELECT * FROM Client WHERE Id = @IdClient",
                        new { IdClient = order.IdClient }).SingleOrDefault();

                    order.Client.Address = new Address();
                    order.Client.Address = standartPersistence.GetEntities<Address>(
                        CommandType.Text,
                        "SELECT * FROM Address WHERE Id = @IdAddress",
                        new { order.IdAddress }).SingleOrDefault();
                }

                return orders;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public List<Order> ListOrders(Company company)
        {
            try
            {
                StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                List<Order> orders = new List<Order>();

                orders = standartPersistence.GetEntities<Order>(
                    CommandType.Text,
                    "SELECT * FROM Order WHERE IdCompany = @IdCompany",
                    new { IdCompany = company.Id }).ToList();

                foreach (var order in orders)
                {
                    order.Company = new Company();
                    order.Company = standartPersistence.GetEntities<Company>(
                        CommandType.Text,
                        "SELECT * FROM Company WHERE Id = @IdCompany",
                        new { IdCompany = order.IdCompany }).SingleOrDefault();

                    order.OrderItems = new List<OrderItem>();
                    order.OrderItems = standartPersistence.GetEntities<OrderItem>(
                        CommandType.Text,
                        "SELECT * FROM OrderItem WHERE IdOrder = @IdOrder",
                        new { IdOrder = order.Id }).ToList();

                    order.Client = new Model.Profile.Client.Entity.B2C.Client();
                    order.Client = standartPersistence.GetEntities<Model.Profile.Client.Entity.B2C.Client>(
                        CommandType.Text,
                        "SELECT * FROM Client WHERE Id = @IdClient",
                        new { IdClient = order.IdClient }).SingleOrDefault();

                    order.Client.Address = new Address();
                    order.Client.Address = standartPersistence.GetEntities<Address>(
                        CommandType.Text,
                        "SELECT * FROM Address WHERE Id = @IdAddress",
                        new { order.IdAddress }).SingleOrDefault();
                }

                return orders;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<Order> ListOrders(DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                List<Order> orders = new List<Order>();

                orders = standartPersistence.GetEntities<Order>(
                    CommandType.Text,
                    "SELECT * FROM Order WHERE DateCreated BETWEEN @DateStart AND @DateEnd",
                    new
                    {
                        dateStart,
                        dateEnd

                    }).ToList();

                foreach (var order in orders)
                {
                    order.Company = new Company();
                    order.Company = standartPersistence.GetEntities<Company>(
                        CommandType.Text,
                        "SELECT * FROM Company WHERE Id = @IdCompany",
                        new { IdCompany = order.IdCompany }).SingleOrDefault();

                    order.OrderItems = new List<OrderItem>();
                    order.OrderItems = standartPersistence.GetEntities<OrderItem>(
                        CommandType.Text,
                        "SELECT * FROM OrderItem WHERE IdOrder = @IdOrder",
                        new { IdOrder = order.Id }).ToList();

                    order.Client = new Model.Profile.Client.Entity.B2C.Client();
                    order.Client = standartPersistence.GetEntities<Model.Profile.Client.Entity.B2C.Client>(
                        CommandType.Text,
                        "SELECT * FROM Client WHERE Id = @IdClient",
                        new { IdClient = order.IdClient }).SingleOrDefault();

                    order.Client.Address = new Address();
                    order.Client.Address = standartPersistence.GetEntities<Address>(
                        CommandType.Text,
                        "SELECT * FROM Address WHERE Id = @IdAddress",
                        new { order.IdAddress }).SingleOrDefault();
                }

                return orders;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public List<OrderEstimate> ListOrderEstimate(Model.Profile.Client.Entity.B2C.Client client)
        {
            try
            {
                StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                List<OrderEstimate> estimates = new List<OrderEstimate>();

                estimates = standartPersistence.GetEntities<OrderEstimate>(
                    CommandType.Text,
                    "SELECT * FROM OrderEstimate WHERE IdClient = @IdClient",
                    new { IdClient = client.Id }).ToList();

                foreach (var estimate in estimates)
                {
                    estimate.Company = new Company();
                    estimate.Company = standartPersistence.GetEntities<Company>(
                        CommandType.Text,
                        "SELECT * FROM Company WHERE Id = @IdCompany",
                        new { IdCompany = estimate.IdCompany }).SingleOrDefault();

                    estimate.OrderEstimateItems = new List<OrderEstimateItem>();
                    estimate.OrderEstimateItems = standartPersistence.GetEntities<OrderEstimateItem>(
                        CommandType.Text,
                        "SELECT * FROM OrderEstimateItem WHERE IdOrderEstimate = @IdOrderEstimate",
                        new { IdOrderEstimate = estimate.Id }).ToList();

                    estimate.Client = new Model.Profile.Client.Entity.B2C.Client();
                    estimate.Client = standartPersistence.GetEntities<Model.Profile.Client.Entity.B2C.Client>(
                        CommandType.Text,
                        "SELECT * FROM Client WHERE Id = @IdClient",
                        new { IdClient = estimate.IdClient }).SingleOrDefault();

                    estimate.Client.Address = new Address();
                    estimate.Client.Address = standartPersistence.GetEntities<Address>(
                        CommandType.Text,
                        "SELECT * FROM Address WHERE Id = @IdAddress",
                        new { estimate.IdAddress }).SingleOrDefault();
                }

                return estimates;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<OrderEstimate> ListOrderEstimate(DateTime dateStart, DateTime dateEnd)
        {
            try
            {

                StandartPersistence standartPersistence =
                         new StandartPersistence(this.Connection);

                List<OrderEstimate> estimates = new List<OrderEstimate>();

                estimates = standartPersistence.GetEntities<OrderEstimate>(
                    CommandType.Text,
                    "SELECT * FROM OrderEstimate WHERE DateCreated BETWEEN @DateStart AND @DateEnd",
                    new
                    {
                        dateStart,
                        dateEnd

                    }).ToList();

                foreach (var estimate in estimates)
                {
                    estimate.Company = new Company();
                    estimate.Company = standartPersistence.GetEntities<Company>(
                        CommandType.Text,
                        "SELECT * FROM Company WHERE Id = @IdCompany",
                        new { IdCompany = estimate.IdCompany }).SingleOrDefault();

                    estimate.OrderEstimateItems = new List<OrderEstimateItem>();
                    estimate.OrderEstimateItems = standartPersistence.GetEntities<OrderEstimateItem>(
                        CommandType.Text,
                        "SELECT * FROM OrderEstimateItem WHERE IdOrderEstimate = @IdOrderEstimate",
                        new { IdOrderEstimate = estimate.Id }).ToList();

                    estimate.Client = new Model.Profile.Client.Entity.B2C.Client();
                    estimate.Client = standartPersistence.GetEntities<Model.Profile.Client.Entity.B2C.Client>(
                        CommandType.Text,
                        "SELECT * FROM Client WHERE Id = @IdClient",
                        new { IdClient = estimate.IdClient }).SingleOrDefault();

                    estimate.Client.Address = new Address();
                    estimate.Client.Address = standartPersistence.GetEntities<Address>(
                        CommandType.Text,
                        "SELECT * FROM Address WHERE Id = @IdAddress",
                        new { estimate.IdAddress }).SingleOrDefault();
                }

                return estimates;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Aceita um orçamento e gera um pedido, ou recusa um orçamento de prestação de serviço. 
        /// </summary>
        /// <param name="order"></param>
        public Order AcceptEstimate(Order order)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    order.OrderEstimate.DateUpdated = DateTime.Now;
                    standartPersistence.Update<OrderEstimate>(order.OrderEstimate);

                    // Caso o orçamento tenha sido aceito, o pedido será iniciado.
                    if (order.OrderEstimate.Accepted)
                    {
                        order.TotalPrice = order.OrderEstimate.OrderEstimateItems.Sum(x => x.UnitPrice);
                        order.OrderStatus = (int)Order.Status.OPEN;
                        order.DateCreated = DateTime.Now;
                        order.DateUpdated = DateTime.Now;
                        order.IdClient = order.OrderEstimate.IdClient;
                        order.IdCompany = order.OrderEstimate.IdCompany;
                        order.IdAddress = order.OrderEstimate.IdAddress;
                        order.IdOrderEstimate = order.OrderEstimate.Id;
                        order.DateScheduled = order.OrderEstimate.DateScheduled;
                        order.HourScheduled = order.OrderEstimate.HourScheduled;

                        standartPersistence.Insert<Order>(order);
                        List<OrderItem> orderItems = new List<OrderItem>();
                        foreach (var item in order.OrderEstimate.OrderEstimateItems)
                        {
                            OrderItem orderItem = new OrderItem();
                            orderItem.DateCreated = DateTime.Now;
                            orderItem.DateUpdated = DateTime.Now;
                            orderItem.IdOrder = order.Id;
                            orderItem.UnitPrice = item.UnitPrice;
                            orderItem.IdExpertise = item.IdExpertise;
                            orderItem.OrderItemStatus = (int)OrderItem.Status.PENDENTE;
                            standartPersistence.Insert<OrderItem>(orderItem);
                            orderItems.Add(orderItem);
                        }
                        order.OrderItems = orderItems;

                        // Gera agenda
                        order.OrderSchedule.IdOrder = order.Id;
                        order.OrderSchedule.IdCompany = order.IdCompany;
                        order.OrderSchedule.DateCreated = DateTime.Now;
                        order.OrderSchedule.DateScheduled = order.DateScheduled;
                        order.OrderSchedule.HourScheduled = order.HourScheduled;
                        standartPersistence.Insert<OrderSchedule>(order.OrderSchedule);
                    }
                    // Caso contrário, não se inicia o pedido e o orçamento será recusado.
                    else
                    {
                        standartPersistence.Update<OrderEstimate>(order.OrderEstimate);
                    }
                    transactionScope.Complete();
                    return order;
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

        /// <summary>
        /// Altera dados de um pedido e seus serviços.
        /// </summary>
        /// <param name="order"></param>
        public void UpdateOrderAndItems(Order order)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    // Caso tenha um serviço cancelado, será subtraído do valor total do pedido.
                    if (order.OrderItems.Exists(x => x.OrderItemStatus == (int)OrderItem.Status.CANCELADO))
                    {
                        order.TotalPrice = order.TotalPrice - order.OrderItems.Sum(x => x.OrderItemStatus == (int)OrderItem.Status.CANCELADO ? x.UnitPrice : 0);
                    }

                    standartPersistence.Update<Order>(order);

                    if (order.OrderItems.Count > 0)
                    {
                        for (int i = 0; i < order.OrderItems.Count; i++)
                        {
                            standartPersistence.Update<OrderItem>(order.OrderItems[i]);
                        }
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

        /// <summary>
        /// Cria um registro de auditoria com fotos de antes e depois de cada serviço no pedido.
        /// </summary>
        /// <param name="order"></param>
        public void CreateAudit(OrderItemAudit orderItemAudit)
        {
            string connection = "DefaultEndpointsProtocol=https;AccountName=genesissalesstorage;AccountKey=9n93XTGDxNLDzsX0FKyChCQsVFFrgmbWYDBjj7o4Hv0lAoAhEBTEOlQQzSnN2IFCw8yq2Mf8P/OZT7TNYBHP9w==";
            string container = "bplace";
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

                    var blob = AzureBlobStorage.Upload(orderItemAudit.FilePath,
                        orderItemAudit.Name, connection, container);

                    if (blob != null)
                    {
                        orderItemAudit.URL = blob.StorageUri.PrimaryUri.AbsoluteUri;
                        orderItemAudit.DateCreated = DateTime.Now;
                        orderItemAudit.DateUpdated = DateTime.Now;
                        standartPersistence.Insert<OrderItemAudit>(orderItemAudit);
                    }
                    else
                    {
                        throw new System.Exception("Falha no processo de Upload de arquivos no AzureBlobStorage.");
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

        public void CancelOrderByEstimate(Order order)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                standartPersistence.Execute(CommandType.Text,
                    $"UPDATE [Order] SET OrderStatus = { (int)Order.Status.CANCEL } WHERE Id = { order.Id } AND IdOrderEstimate = { order.OrderEstimate.Id }");
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        public void CloseOrder(int idOrder, int discount)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);
                    string txt = $"select * from [Order] where id = {idOrder}";
                    Order order = new Order();
                    order = standartPersistence.GetEntities<Order>(CommandType.Text, txt).FirstOrDefault();

                    if (order != null)
                    {
                        order.DateUpdated = DateTime.Now;
                        // order.DateServiceStart = DateTime.Now;
                        order.DateServiceFinal = DateTime.Now;
                        order.OrderStatus = (int)Order.Status.ClOSED;
                        standartPersistence.Update<Order>(order);


                        if (order.OrderStatus == (int)Order.Status.ClOSED)
                        {
                            // Inicia um fatura

                            /* ESSA PARTE PRECISARÁ SER REPENSADA, POIS O PAGAMENTO TERÁ UMA API PRÓPRIA E FARÁ INCLUSÃO DE MANEIRA INDEPENDENTE.
                            order.PaymentInvoice = new PaymentTransaction();
                            order.PaymentInvoice.IdOrder = order.Id;
                            order.PaymentInvoice.Type = (int)PaymentTransaction.NegotiationType.B2C;
                            order.PaymentInvoice.PaymentInvoiceStatus = (int)PaymentTransaction.Status.PENDENTE;
                            order.PaymentInvoice.Amount = order.TotalPrice;
                            order.PaymentInvoice.Discount = discount;
                            order.PaymentInvoice.Net = order.TotalPrice - order.PaymentInvoice.Discount;
                            order.PaymentInvoice.DateCreated = DateTime.Now;
                            order.PaymentInvoice.DateUpdated = DateTime.Now;
                            standartPersistence.Insert<PaymentTransaction>(order.PaymentInvoice);

                             */
                        }
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

        public void StartFeedback(Order order)
        {
            try
            {
                StandartPersistence standartPersistence =
                    new StandartPersistence(this.Connection);

                standartPersistence.Insert<OrderFeedback>(order.OrderFeedbacks[0]);
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void StartExpedient(OrderExpedient orderExpedient)
        {
            StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);
            orderExpedient.IdStatus = 1;
            orderExpedient.StartDate = DateTime.Now;

            standartPersistence.Insert<OrderExpedient>(orderExpedient);

        }

        public void CloseExpedient(OrderExpedient orderExpedient)
        {
            StandartPersistence standartPersistence =
                        new StandartPersistence(this.Connection);

            string txt = $"Update orderexpedient set idstatus=2,enddate='{DateTime.Now}',comment='{orderExpedient.Comment}' where id={orderExpedient.Id}";
            standartPersistence.Execute(CommandType.Text, txt);

        }

        public List<OrderSchedule> ListOrderSchedule(int idCompany, DateTime dateStart, DateTime dateEnd)
        {
            StandartPersistence standartPersistence =
                         new StandartPersistence(this.Connection);

            List<OrderSchedule> orderSchedules = new List<OrderSchedule>();

            orderSchedules = standartPersistence.GetEntities<OrderSchedule>(
                CommandType.Text,
                "SELECT * FROM OrderSchedule WHERE IdCompany = @IdCompany AND DateScheduled BETWEEN @DateStart AND @DateEnd",
                new
                {
                    idCompany,
                    dateStart,
                    dateEnd

                }).ToList();

            return orderSchedules;
        }
    }
}

