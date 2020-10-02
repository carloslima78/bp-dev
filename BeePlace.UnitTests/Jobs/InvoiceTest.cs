using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using BeePlace.Services.Revenues;

namespace BeeChurch.UnitTests.Jobs
{
    [TestClass]
    public class InvoiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            InvoiceService invoiceService = new InvoiceService(ConfigurationManager.ConnectionStrings["BeePlace"].ConnectionString);
            invoiceService.CreateInvoice();
        }
    }
}
