using ConversionPath.Application.ExchangeRates.Queries;
using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Shared.Dtos;
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
        var simpleRoute = allRates
            .FirstOrDefault(r => r.SourceCurrency.ToLower().Equals(sourceCurrency.ToLower())
             && r.DestinationCurrency.ToLower().Equals(destinationCurrency.ToLower()));
        if (simpleRoute != null)
        {
            result.IsSucessfull = true;
            result.RatesUsed.Add(simpleRoute);
            result.Result = amount * simpleRoute.Rate;
        }
        else
        {
            
        }
        return result;
    }
}