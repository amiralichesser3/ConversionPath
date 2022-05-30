using ConversionPath.Shared.Dtos;
using ConversionPath.Shared.Dtos.ExchangeRates;

namespace ConversionPath.Domain.Contracts;

public interface ICurrencyConverter
{
    void SetRates(ICollection<ExchangeRateDto> rates);
    void ResetRates();
    Task<ConversionResultDto> Convert(string sourceCurrency, string destinationCurrency, double amount);
}