using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;

namespace ConversionPath.Domain.DomainModels.ExchangeRates
{
    public class ExchangeRateCollection : DomainCollection<ExchangeRate>
    {
        public ExchangeRateCollection(IValidator<ExchangeRate> validator) : base(validator)
        {
        }

        public override async Task<ValidationResult<ExchangeRate>> Update(int id, ExchangeRate item)
        {
            var validationResult = await _validator.Validate(item);
            if (!validationResult.IsSuccessfull)
            {
                return validationResult;
            }
            var exchangeRate = Items.First(r => r.Id == id);
            exchangeRate.SourceCurrency = item.SourceCurrency;
            exchangeRate.DestinationCurrency = item.DestinationCurrency;
            exchangeRate.DateTime = item.DateTime;
            validationResult.Data = exchangeRate;
            return validationResult;
        }
    }
}
