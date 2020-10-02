using BeePlace.Infra.DataBaseUtils;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace BeePlace.Model.Geolocation.ValueObject
{
    public class City 
    {
        public City()
        { }

        [DapperKey]
        public int IdCity { get; set; }

        public string Name { get; set; }

        public int IdEstate { get; set; }

        public List<string> Validate()
        {
            var validationResults = new CityValitator().Validate(this);
            var errors = new List<string>();
            foreach (var error in validationResults.Errors)
            {
                errors.Add(error.ErrorMessage);
            }

            return errors;
        }
    }

    public class CityValitator : AbstractValidator<City>
    {
        public override ValidationResult Validate(ValidationContext<City> context)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Não pode nulo");
            return base.Validate(context);
        }
    }
}
