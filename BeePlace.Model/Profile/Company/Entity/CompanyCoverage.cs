using BeePlace.Infra.DataBaseUtils;
using System;

namespace BeePlace.Model.Profile.Company.Entity
{
    public class CompanyCoverageArea
    {
        public int IdCompany { get; set; }
        public string StateCode { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
    }
}
