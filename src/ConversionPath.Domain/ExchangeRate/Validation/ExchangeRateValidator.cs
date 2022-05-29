using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionPath.Domain.ExchangeRates.Validation
{
    public class ExchangeRateValidator : IValidator<ExchangeRate>
    {
        public Task<ValidationResult<ExchangeRate>> Validate(ExchangeRate? input)
        {
            var result = new ValidationResult<ExchangeRate>
            {
                Data = input,
                IsSuccessfull = true,
                Messages = new List<string>()
            };

            if (input == null)
            {
                result.IsSuccessfull = false;
                result.Messages.Add("Exchange Rate Cannot be Null");
                return Task.FromResult(result);
            }

            if (string.IsNullOrEmpty(input.SourceCurrency) || string.IsNullOrEmpty(input.DestinationCurrency)
                || input.DateTime == default || input.Rate == default)
            {
                result.IsSuccessfull = false;
                result.Messages.Add("Exchange Rate Cannot have null values");
            }

            if (input.Rate <= 0)
            {
                result.IsSuccessfull = false;
                result.Messages.Add("Invalid Rate");
            } 

            return Task.FromResult(result);
        }
    }
}
