using ConversionPath.Shared.Dtos;

namespace ConversionPath.Domain.Contracts;

public interface ICurrencyConverter
{
    Task<ConversionResultDto> Convert(string sourceCurrency, string destinationCurrency, double amount);
}