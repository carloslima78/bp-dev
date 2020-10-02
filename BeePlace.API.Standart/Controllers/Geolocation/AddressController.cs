using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BeePlace.Model.Geolocation.Entity;
using BeePlace.Services.Geolocation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Json;
using Microsoft.Extensions.Configuration;

namespace BeePlace.API.Standart.Controllers.Geolocation
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
       
        [HttpGet("{zip}")]
        public ActionResult<ObjectResult> ValidatedAddress(string zip)
        {
            try
            {
                Address address = new Address();
                AddressService addressService = new AddressService(Startup.BeePlaceDataBaseConnectionString);
                address = addressService.GetValidatedAddress(zip);
                return StatusCode((int)HttpStatusCode.OK, address);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        public ActionResult<ObjectResult> AllEstates()
        {
            try
            {
                Address address = new Address();
                AddressService addressService = new AddressService(Startup.BeePlaceDataBaseConnectionString);
                address = addressService.GetEstates();
                return StatusCode((int)HttpStatusCode.OK, address);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{idEstate}")]
        public ActionResult<ObjectResult> CitiesByEstate(int idEstate)
        {
            try
            {
                Address address = new Address();
                AddressService addressService = new AddressService(Startup.BeePlaceDataBaseConnectionString);
                address = addressService.GetCities(idEstate);
                return StatusCode((int)HttpStatusCode.OK, address);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}




