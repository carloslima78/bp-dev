using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BeePlace.Model.Geolocation.ValueObject;
using BeePlace.Services.Geolocation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeePlace.API.Standart.Controllers.Geolocation
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ObjectResult> GetStates()
        {
            StateService stateService = new StateService(Startup.BeePlaceDataBaseConnectionString);
            StateService stateServiceLocalization = new StateService(Startup.BeePlaceLocalizationConnectionString);

            List<State> state = new List<State>();

            state = stateService.GetState();
            return StatusCode((int)HttpStatusCode.OK, state);
        }

        [HttpGet]
        public ActionResult<ObjectResult> GetCities(int IdState, string City)
        {
            StateService stateService = new StateService(Startup.BeePlaceDataBaseConnectionString);
            City cities = new City();

            cities = stateService.GetCities(IdState,City);
            return StatusCode((int)HttpStatusCode.OK, cities);
        }

        [HttpGet]
        public ActionResult<ObjectResult> GetDistricts(string cityName, string stateCode)
        {
            StateService stateService = new StateService(Startup.BeePlaceLocalizationConnectionString);
            List<District> districts = new List<District>();

            districts = stateService.GetDistricts(cityName, stateCode);
            return StatusCode((int)HttpStatusCode.OK, districts);
        }

        [HttpGet]
        public ActionResult<ObjectResult> GetDistrictsByZip(string zip)
        {
            StateService stateService = new StateService(Startup.BeePlaceLocalizationConnectionString);
            District district = new District();

            district = stateService.GetDistrictsByZip(zip);
            return StatusCode((int)HttpStatusCode.OK, district);
        }
    }
}