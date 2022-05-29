using ConversionPath.Application.ExchangeRates.Queries;
using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Shared.Dtos;
using ConversionPath.Shared.Dtos.ExchangeRates;
using MediatR;

namespace ConversionPath.Application.Conversion;

public class CurrencyConverter: ICurrencyConverter
{
    private readonly IMediator _mediator;


    public CurrencyConverter(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ConversionResultDto> Convert(string sourceCurrency, string destinationCurrency, double amount)
    {
        var result = new ConversionResultDto();
        var allRates = await _mediator.Send(new GetAllExchangeRatesQuery());
       
        await FindPath(allRates, sourceCurrency, destinationCurrency);
        if (allRates.Any())
        {
            result.IsSucessfull = true;
            result.RatesUsed = allRates;
            if (allRates.Count == 1)
            {
                result.Result = allRates.First().Rate * amount;
            }
        }
        return result;
    }

    private async Task FindPath(ICollection<ExchangeRateDto> allRates, string sourceCurrency, string destinationCurrency)
    {
        var simpleRoute = allRates
            .FirstOrDefault(r => r.SourceCurrency.ToLower().Equals(sourceCurrency.ToLower())
                                 && r.DestinationCurrency.ToLower().Equals(destinationCurrency.ToLower()))?.Clone();
        if (simpleRoute != null)
        {
            allRates.Clear();
            allRates.Add(simpleRoute);
            return;
        }
        else
        { 
            var possibleStart = allRates.Where(r => 
                r.SourceCurrency.ToLower().Equals(sourceCurrency.ToLower())
                || r.SourceCurrency.ToLower().Equals(destinationCurrency.ToLower())
                || r.DestinationCurrency.ToLower().Equals(destinationCurrency.ToLower())
                || r.DestinationCurrency.ToLower().Equals(sourceCurrency.ToLower())
            ).ToList();
        }
    }
}