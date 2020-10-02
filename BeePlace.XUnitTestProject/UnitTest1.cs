using BeePlace.API.Standart.Controllers.Geolocation;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;


namespace BeePlace.XUnitTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            try
            {
                AddressController addressController = new AddressController();
                addressController.AllEstates();
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
